using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zotin
{

    public static class GrayScale
    {
        public static byte FromRgb(byte r, byte g, byte b)
        {
            return (byte)(r * 0.30f + g * 0.59f + b * 0.11f);
        }

        public static byte FromYuv(byte r, byte g, byte b)
        {
            //Y channel
            return (byte)(ClassColorModel.Kr * r + 0.587 * g + ClassColorModel.Kb * b);
        }

        public static byte FromHsv(byte r, byte g, byte b)
        {
            byte max = Math.Max(r, Math.Max(g, b));
            byte min = Math.Min(r, Math.Min(g, b));
            //S channel
            return (byte)(max == 0 ? 0 : 1 - (1d * min / max) * 255);
        }
    }

    public class WorkItem
    {
        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }
        public string Function { get; set; }
        public double Time { get; set; }
    }

    public class SegmentInfo
    {
        public int Area { get; set; }
        public int Perimeter { get; set; }
        public Point CenterOfMass { get; set; }
        public double Density { get; set; }
        public String ChainCode8 { get; set; }


        public SegmentInfo(Point[] points)
        {
            #region Area
            Area = points.Length;
            #endregion

            #region Perimeter
            var yGroups = points.GroupBy(item => item.Y).ToArray();
            List<Point> list = yGroups.Select(group => group.First()).ToList();
            for (int index = yGroups.Length - 1; index >= 0; index--)
            {
                Point point = yGroups[index].Last();
                list.Add(point);
            }
            //Close the perimeter
            list.Add(list.First());

            Perimeter = 0;
            for (int i = 1; i < list.Count; i++)
            {
                Point point1 = list[i - 1];
                Point point2 = list[i];
                Perimeter += Math.Abs(point2.X - point1.X) + 1;
            }
            #endregion

            #region CenterOfMass
            int centerX = points.Sum(point => point.X) / Area;
            int centerY = points.Sum(point => point.Y) / Area;
            CenterOfMass = new Point(centerX, centerY);
            #endregion

            #region Density
            Density = (double)Perimeter / Area;
            #endregion

            #region ChainCode
            StringBuilder sb8 = new StringBuilder();
            for (int i = 1; i < list.Count; i++)
            {
                Point point1 = list[i - 1];
                Point point2 = list[i];
                int x1 = point1.X;
                int y1 = point1.Y;
                int x2 = point2.X;
                int y2 = point2.Y;
                while (x1 != x2 || y1 != y2)
                {
                    if (x1 < x2)
                    {
                        x1++;
                        if (y1 < y2)
                        {
                            sb8.Append("5");
                            y1++;
                        }
                        else if (y1 > y2)
                        {
                            y1--;
                            sb8.Append("3");
                        }
                        else
                        {
                            sb8.Append("4");
                        }
                    }
                    else if (x1 > x2)
                    {
                        x1--;
                        if (y1 < y2)
                        {
                            y1++;
                            sb8.Append("7");
                        }
                        else if (y1 > y2)
                        {
                            y1--;
                            sb8.Append("1");
                        }
                        else
                        {
                            sb8.Append("0");
                        }
                    }
                    else
                    {
                        if (y1 > y2)
                        {
                            y1--;
                            sb8.Append("2");
                        }
                        else if (y1 < y2)
                        {
                            y1++;
                            sb8.Append("6");
                        }
                    }
                }
            }
            ChainCode8 = sb8.ToString();
            #endregion
        }


        public override string ToString()
        {
            //return string.Format("Area = {0}, Perimeter = {1}, CenterOfMass = {2}, Density = {3}, ChainCode = {4}", Area, Perimeter, CenterOfMass, Density, ChainCode8);
            return null;
        }
    }
    public class MetricsInfo
    {
        public double Mse { get; set; }
        public double Msad { get; set; }
        public double Psnr { get; set; }
    }
    public class HarrisPoint : IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double R { get; set; }

        public HarrisPoint(int x, int y, double r)
        {
            X = x;
            Y = y;
            R = r;
        }

        public int CompareTo(object obj)
        {
            HarrisPoint p2 = (HarrisPoint)obj;
            return (int)(R - p2.R);
        }

        public double Dist(HarrisPoint point)
        {
            return Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));
        }
    }
}

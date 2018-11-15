using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zotin
{
    public class SegmentsComparer : ImageProcessor
    {
        public IDictionary<int, List<Point>> Regions { get; set; }

        //Region growing alogrithm
        public Bitmap SegmentationSelectSegments()
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int[][] labels = new int[bitmapData.Height][];
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i] = new int[bitmapData.Width];
                }
                Regions = new Dictionary<int, List<Point>>();
                int labelNumber = 0;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int index = row * Stride + col * 3;
                        int color = ColorModel.GetColor(buffer[index + 2], buffer[index + 1], buffer[index]);
                        int colorTop = row > 0 ? ColorModel.GetColor(buffer[index + 2 - Stride], buffer[index + 1 - Stride], buffer[index - Stride]) : -1;
                        int colorLeft = col > 0 ? ColorModel.GetColor(buffer[index - 1], buffer[index - 2], buffer[index - 3]) : -1;
                        if (row > 0 && col > 0 && color == colorTop && color == colorLeft)
                        {
                            var label1 = labels[row][col - 1];
                            var label2 = labels[row - 1][col];
                            labels[row][col] = label1;
                            Regions[label1].Add(new Point(col, row));
                            if (label1 != label2)
                            {
                                Regions[label1].AddRange(Regions[label2]);
                                foreach (var point in Regions[label2])
                                {
                                    labels[point.Y][point.X] = label1;
                                }
                                Regions.Remove(label2);
                            }
                        }
                        else if (col > 0 && color == colorLeft)
                        {
                            int label = labels[row][col - 1];
                            labels[row][col] = label;
                            Regions[label].Add(new Point(col, row));
                        }
                        else if (row > 0 && color == colorTop)
                        {
                            int label = labels[row - 1][col];
                            labels[row][col] = label;
                            Regions[label].Add(new Point(col, row));
                        }
                        else
                        {
                            labelNumber++;
                            Regions[labelNumber] = new List<Point>
                            {
                                new Point(col, row)
                            };
                            labels[row][col] = labelNumber;
                        }
                    }
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zotin
{
    class Segmentation
    {
        public static Bitmap applyRobertsSegmentation(string colorModel, int currentChannel, Bitmap image, Point imageSize, float amplifier = 1, double threshold = 10)
        {
            Bitmap rezult = new Bitmap(image);

            for (int x = 0; x < imageSize.X; x++)
            {
                for (int y = 0; y < imageSize.Y; y++)
                {
                    double xSumR = 0, xSumG = 0, xSumB = 0;
                    double ySumR = 0, ySumG = 0, ySumB = 0;

                    int rightX = x + 1;
                    int bottomY = y + 1;

                    bool hasBottom = bottomY < imageSize.Y;
                    bool hasRight = rightX < imageSize.X;
                    Color color;

                    float factor = -1;

                    if (hasBottom)
                    {
                        if (hasRight)
                        {
                            color = image.GetPixel(rightX, bottomY);
                            xSumR += color.R * factor;
                            xSumG += color.G * factor;
                            xSumB += color.B * factor;
                        }
                        else
                        {
                            color = image.GetPixel(x, bottomY);
                            xSumR += color.R * factor;
                            xSumG += color.G * factor;
                            xSumB += color.B * factor;
                        }

                        color = image.GetPixel(x, bottomY);
                        ySumR += color.R * factor;
                        ySumG += color.G * factor;
                        ySumB += color.B * factor;
                    }
                    else
                    {

                        if (hasRight)
                        {
                            color = image.GetPixel(rightX, y);
                            xSumR += color.R * factor;
                            xSumG += color.G * factor;
                            xSumB += color.B * factor;
                        }
                        else
                        {
                            color = image.GetPixel(x, y);
                            xSumR += color.R * factor;
                            xSumG += color.G * factor;
                            xSumB += color.B * factor;
                        }

                        color = image.GetPixel(x, y);
                        ySumR += color.R * factor;
                        ySumG += color.G * factor;
                        ySumB += color.B * factor;
                    }

                    factor = 1;

                    if (hasRight)
                    {
                        color = image.GetPixel(rightX, y);
                        ySumR += color.R * factor;
                        ySumG += color.G * factor;
                        ySumB += color.B * factor;
                    }
                    else
                    {
                        color = image.GetPixel(x, y);
                        ySumR += color.R * factor;
                        ySumG += color.G * factor;
                        ySumB += color.B * factor;
                    }

                    color = image.GetPixel(x, y);
                    xSumR += color.R * factor;
                    xSumG += color.G * factor;
                    xSumB += color.B * factor;


                    double resultR = amplifier * Math.Sqrt(Math.Pow(xSumR, 2) + Math.Pow(ySumR, 2));
                    double resultG = amplifier * Math.Sqrt(Math.Pow(xSumG, 2) + Math.Pow(ySumG, 2));
                    double resultB = amplifier * Math.Sqrt(Math.Pow(xSumB, 2) + Math.Pow(ySumB, 2));

                    if (resultR < threshold) resultR = 0;
                    if (resultG < threshold) resultG = 0;
                    if (resultB < threshold) resultB = 0;

                    if (resultR > 255) resultR = 255;
                    if (resultG > 255) resultG = 255;
                    if (resultB > 255) resultB = 255;


                    Color oldColor = image.GetPixel(x, y);
                    if (currentChannel == 0 || currentChannel == 4)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb((int)resultR, (int)resultG, (int)resultB));
                    }
                    else if (currentChannel == 1)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb((int)resultR, oldColor.G, oldColor.B));
                    }
                    else if (currentChannel == 2)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb(oldColor.R, (int)resultG, oldColor.B));
                    }
                    else if (currentChannel == 3)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb(oldColor.R, oldColor.G, (int)resultB));
                    }
                }
            }

            return rezult;
        }

        public static Bitmap applySobelSegmentation(string colorModel, int currentChannel, Bitmap image, Point imageSize, float amplifier = 1, double threshold = 10)
        {
            Bitmap rezult = new Bitmap(image);

            //привет овсян, проверим твой хак
            rezult = applyRobertsSegmentation(colorModel, currentChannel, image, imageSize, amplifier * 2, threshold);

            return rezult;
        }

        public static Bitmap applyCannySegmentation2(string colorModel, int currentChannel, Bitmap image, Point imageSize, Bitmap Gauss, float amplifier = 1, double threshold = 10)
        {
            Bitmap rezult = new Bitmap(image);
            double sigma = 2.2;

            List<List<double>> gradxR = new List<List<double>>();
            List<List<double>> gradyR = new List<List<double>>();
            List<List<double>> gradxG = new List<List<double>>();
            List<List<double>> gradyG = new List<List<double>>();
            List<List<double>> gradxB = new List<List<double>>();
            List<List<double>> gradyB = new List<List<double>>();

            int[][] sobel_x = new int[3][];
            sobel_x[0] = new int[3] { -1, 0, 1 };
            sobel_x[1] = new int[3] { -2, 0, 2 };
            sobel_x[2] = new int[3] { -1, 0, 1 };

            int[][] sobel_y = new int[3][];
            sobel_y[0] = new int[3] { -1, -2, -1 };
            sobel_y[1] = new int[3] { 0, 0, 0 };
            sobel_y[2] = new int[3] { 1, 2, 1 };

            int width = imageSize.X;
            int height = imageSize.Y;

            for (int x = 1; x < width - 1; x++)
            {
                List<double> gradLinexR = new List<double>();
                List<double> gradLineyR = new List<double>();
                List<double> gradLinexG = new List<double>();
                List<double> gradLineyG = new List<double>();
                List<double> gradLinexB = new List<double>();
                List<double> gradLineyB = new List<double>();

                for (int y = 1; y < height - 1; y++)
                {
                    double pxR = ((sobel_x[0][0] * Gauss.GetPixel(x - 1, y - 1).R) + (sobel_x[0][1] * Gauss.GetPixel(x, y - 1).R) + (sobel_x[0][2] * Gauss.GetPixel(x + 1, y - 1).R)
                                + (sobel_x[1][0] * Gauss.GetPixel(x - 1, y - 0).R) + (sobel_x[1][1] * Gauss.GetPixel(x, y - 0).R) + (sobel_x[1][2] * Gauss.GetPixel(x + 1, y - 0).R)
                                + (sobel_x[2][0] * Gauss.GetPixel(x - 1, y + 1).R) + (sobel_x[2][1] * Gauss.GetPixel(x, y + 1).R) + (sobel_x[2][2] * Gauss.GetPixel(x + 1, y + 1).R));

                    double pyR = ((sobel_y[0][0] * Gauss.GetPixel(x - 1, y - 1).R) + (sobel_y[0][1] * Gauss.GetPixel(x, y - 1).R) + (sobel_y[0][2] * Gauss.GetPixel(x + 1, y - 1).R)
                                + (sobel_y[1][0] * Gauss.GetPixel(x - 1, y - 0).R) + (sobel_y[1][1] * Gauss.GetPixel(x, y - 0).R) + (sobel_y[1][2] * Gauss.GetPixel(x + 1, y - 0).R)
                                + (sobel_y[2][0] * Gauss.GetPixel(x - 1, y + 1).R) + (sobel_y[2][1] * Gauss.GetPixel(x, y + 1).R) + (sobel_y[2][2] * Gauss.GetPixel(x + 1, y + 1).R));

                    double pxG = ((sobel_x[0][0] * Gauss.GetPixel(x - 1, y - 1).G) + (sobel_x[0][1] * Gauss.GetPixel(x, y - 1).G) + (sobel_x[0][2] * Gauss.GetPixel(x + 1, y - 1).G)
                                + (sobel_x[1][0] * Gauss.GetPixel(x - 1, y - 0).G) + (sobel_x[1][1] * Gauss.GetPixel(x, y - 0).G) + (sobel_x[1][2] * Gauss.GetPixel(x + 1, y - 0).G)
                                + (sobel_x[2][0] * Gauss.GetPixel(x - 1, y + 1).G) + (sobel_x[2][1] * Gauss.GetPixel(x, y + 1).G) + (sobel_x[2][2] * Gauss.GetPixel(x + 1, y + 1).G));

                    double pyG = ((sobel_y[0][0] * Gauss.GetPixel(x - 1, y - 1).G) + (sobel_y[0][1] * Gauss.GetPixel(x, y - 1).G) + (sobel_y[0][2] * Gauss.GetPixel(x + 1, y - 1).G)
                                + (sobel_y[1][0] * Gauss.GetPixel(x - 1, y - 0).G) + (sobel_y[1][1] * Gauss.GetPixel(x, y - 0).G) + (sobel_y[1][2] * Gauss.GetPixel(x + 1, y - 0).G)
                                + (sobel_y[2][0] * Gauss.GetPixel(x - 1, y + 1).G) + (sobel_y[2][1] * Gauss.GetPixel(x, y + 1).G) + (sobel_y[2][2] * Gauss.GetPixel(x + 1, y + 1).G));

                    double pxB = ((sobel_x[0][0] * Gauss.GetPixel(x - 1, y - 1).B) + (sobel_x[0][1] * Gauss.GetPixel(x, y - 1).B) + (sobel_x[0][2] * Gauss.GetPixel(x + 1, y - 1).B)
                                + (sobel_x[1][0] * Gauss.GetPixel(x - 1, y - 0).B) + (sobel_x[1][1] * Gauss.GetPixel(x, y - 0).B) + (sobel_x[1][2] * Gauss.GetPixel(x + 1, y - 0).B)
                                + (sobel_x[2][0] * Gauss.GetPixel(x - 1, y + 1).B) + (sobel_x[2][1] * Gauss.GetPixel(x, y + 1).B) + (sobel_x[2][2] * Gauss.GetPixel(x + 1, y + 1).B));

                    double pyB = ((sobel_y[0][0] * Gauss.GetPixel(x - 1, y - 1).B) + (sobel_y[0][1] * Gauss.GetPixel(x, y - 1).B) + (sobel_y[0][2] * Gauss.GetPixel(x + 1, y - 1).B)
                                + (sobel_y[1][0] * Gauss.GetPixel(x - 1, y - 0).B) + (sobel_y[1][1] * Gauss.GetPixel(x, y - 0).B) + (sobel_y[1][2] * Gauss.GetPixel(x + 1, y - 0).B)
                                + (sobel_y[2][0] * Gauss.GetPixel(x - 1, y + 1).B) + (sobel_y[2][1] * Gauss.GetPixel(x, y + 1).B) + (sobel_y[2][2] * Gauss.GetPixel(x + 1, y + 1).B));

                    gradLinexR.Add(pxR);
                    gradLineyR.Add(pyR);

                    gradLinexG.Add(pxG);
                    gradLineyG.Add(pyG);

                    gradLinexB.Add(pxB);
                    gradLineyB.Add(pyB);
                }
                gradxR.Add(gradLinexR);
                gradyR.Add(gradLineyR);

                gradxG.Add(gradLinexG);
                gradyG.Add(gradLineyG);

                gradxB.Add(gradLinexB);
                gradyB.Add(gradLineyB);
            }

            double[,] sobeloutmagR = new double[gradxR.Count, gradxR[0].Count];
            double[,] sobeloutdirR = new double[gradxR.Count, gradxR[0].Count];

            double[,] sobeloutmagG = new double[gradxR.Count, gradxR[0].Count];
            double[,] sobeloutdirG = new double[gradxR.Count, gradxR[0].Count];

            double[,] sobeloutmagB = new double[gradxR.Count, gradxR[0].Count];
            double[,] sobeloutdirB = new double[gradxR.Count, gradxR[0].Count];

            for (int i = 0; i < gradxR.Count; i++)
            {
                for (int j = 0; j < gradxR[i].Count; j++)
                {
                    sobeloutmagR[i, j] = Math.Sqrt(Math.Pow(gradxR[i][j], 2) + Math.Pow(gradyR[i][j], 2));
                    sobeloutdirR[i, j] = Math.Atan2(gradyR[i][j], gradxR[i][j]);

                    sobeloutmagG[i, j] = Math.Sqrt(Math.Pow(gradxG[i][j], 2) + Math.Pow(gradyG[i][j], 2));
                    sobeloutdirG[i, j] = Math.Atan2(gradyG[i][j], gradxG[i][j]);

                    sobeloutmagB[i, j] = Math.Sqrt(Math.Pow(gradxB[i][j], 2) + Math.Pow(gradyB[i][j], 2));
                    sobeloutdirB[i, j] = Math.Atan2(gradyB[i][j], gradxB[i][j]);
                }
            }

            for (int x = 0; x < width - 2; x++)
            {
                for (int y = 0; y < height - 2; y++)
                {
                    if ((sobeloutdirR[x, y] < 22.5 && sobeloutdirR[x, y] >= 0)
                        || (sobeloutdirR[x, y] >= 157.5 && sobeloutdirR[x, y] < 202.5)
                        || (sobeloutdirR[x, y] >= 337.5 && sobeloutdirR[x, y] < 360))
                    {
                        sobeloutdirR[x, y] = 0;
                    }
                    else if ((sobeloutdirR[x, y] >= 22.5 && sobeloutdirR[x, y] < 67.5)
                        || (sobeloutdirR[x, y] >= 202.5 && sobeloutdirR[x, y] < 247.5))
                    {
                        sobeloutdirR[x, y] = 45;
                    }
                    else if ((sobeloutdirR[x, y] >= 67.5 && sobeloutdirR[x, y] < 112.5)
                            || sobeloutdirR[x, y] > 247.5 && sobeloutdirR[x, y] < 292.5)
                    {
                        sobeloutdirR[x, y] = 90;
                    }
                    else
                    {
                        sobeloutdirR[x, y] = 135;
                    }

                    if ((sobeloutdirG[x, y] < 22.5 && sobeloutdirG[x, y] >= 0)
                           || (sobeloutdirG[x, y] >= 157.5 && sobeloutdirG[x, y] < 202.5)
                           || (sobeloutdirG[x, y] >= 337.5 && sobeloutdirG[x, y] < 360))
                    {
                        sobeloutdirG[x, y] = 0;
                    }
                    else if ((sobeloutdirG[x, y] >= 22.5 && sobeloutdirG[x, y] < 67.5)
                            || (sobeloutdirG[x, y] >= 202.5 && sobeloutdirG[x, y] < 247.5))
                    {
                        sobeloutdirG[x, y] = 45;
                    }
                    else if ((sobeloutdirG[x, y] >= 67.5 && sobeloutdirG[x, y] < 112.5)
                            || sobeloutdirG[x, y] > 247.5 && sobeloutdirG[x, y] < 292.5)
                    {
                        sobeloutdirG[x, y] = 90;
                    }
                    else
                    {
                        sobeloutdirG[x, y] = 135;
                    }

                    if ((sobeloutdirB[x, y] < 22.5 && sobeloutdirB[x, y] >= 0)
                            || (sobeloutdirB[x, y] >= 157.5 && sobeloutdirB[x, y] < 202.5)
                            || (sobeloutdirB[x, y] >= 337.5 && sobeloutdirB[x, y] < 360))
                    {
                        sobeloutdirB[x, y] = 0;
                    }
                    else if ((sobeloutdirB[x, y] >= 22.5 && sobeloutdirB[x, y] < 67.5)
                            || (sobeloutdirB[x, y] >= 202.5 && sobeloutdirB[x, y] < 247.5))
                    {
                        sobeloutdirB[x, y] = 45;
                    }
                    else if ((sobeloutdirB[x, y] >= 67.5 && sobeloutdirB[x, y] < 112.5)
                            || sobeloutdirB[x, y] > 247.5 && sobeloutdirB[x, y] < 292.5)
                    {
                        sobeloutdirB[x, y] = 90;
                    }
                    else
                    {
                        sobeloutdirB[x, y] = 135;
                    }
                }
            }

            double[,] mag_supR = new double[gradxR.Count, gradxR[0].Count];
            double[,] mag_supG = new double[gradxR.Count, gradxR[0].Count];
            double[,] mag_supB = new double[gradxR.Count, gradxR[0].Count];
            for (int i = 0; i < gradxR.Count; i++)
            {
                for (int j = 0; j < gradxR[0].Count; j++)
                {
                    mag_supR[i, j] = sobeloutmagR[i, j];
                    mag_supG[i, j] = sobeloutmagG[i, j];
                    mag_supB[i, j] = sobeloutmagB[i, j];
                }
            }

            for (int x = 1; x < width - 3; x++)
            {
                for (int y = 1; y < height - 3; y++)
                {
                    if (sobeloutdirR[x, y] == 0)
                    {
                        if ((sobeloutmagR[x, y] <= sobeloutmagR[x, y + 1])
                            || (sobeloutmagR[x, y] <= sobeloutmagR[x, y - 1]))
                        {
                            mag_supR[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirR[x, y] == 45)
                    {
                        if ((sobeloutmagR[x, y] <= sobeloutmagR[x - 1, y + 1])
                            || (sobeloutmagR[x, y] <= sobeloutmagR[x + 1, y - 1]))
                        {
                            mag_supR[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirR[x, y] == 90)
                    {
                        if ((sobeloutmagR[x, y] <= sobeloutmagR[x + 1, y])
                            || (sobeloutmagR[x, y] <= sobeloutmagR[x - 1, y]))
                        {
                            mag_supR[x, y] = 0;
                        }
                    }
                    else
                    {
                        if ((sobeloutmagR[x, y] <= sobeloutmagR[x + 1, y + 1])
                            || (sobeloutmagR[x, y] <= sobeloutmagR[x - 1, y - 1]))
                        {
                            mag_supR[x, y] = 0;
                        }
                    }

                    if (sobeloutdirG[x, y] == 0)
                    {
                        if ((sobeloutmagG[x, y] <= sobeloutmagG[x, y + 1])
                            || (sobeloutmagG[x, y] <= sobeloutmagG[x, y - 1]))
                        {
                            mag_supG[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirG[x, y] == 45)
                    {
                        if ((sobeloutmagG[x, y] <= sobeloutmagG[x - 1, y + 1])
                            || (sobeloutmagG[x, y] <= sobeloutmagG[x + 1, y - 1]))
                        {
                            mag_supG[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirG[x, y] == 90)
                    {
                        if ((sobeloutmagG[x, y] <= sobeloutmagG[x + 1, y])
                            || (sobeloutmagG[x, y] <= sobeloutmagG[x - 1, y]))
                        {
                            mag_supG[x, y] = 0;
                        }
                    }
                    else
                    {
                        if ((sobeloutmagG[x, y] <= sobeloutmagG[x + 1, y + 1])
                            || (sobeloutmagG[x, y] <= sobeloutmagG[x - 1, y - 1]))
                        {
                            mag_supG[x, y] = 0;
                        }
                    }

                    if (sobeloutdirB[x, y] == 0)
                    {
                        if ((sobeloutmagB[x, y] <= sobeloutmagB[x, y + 1])
                            || (sobeloutmagB[x, y] <= sobeloutmagB[x, y - 1]))
                        {
                            mag_supB[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirB[x, y] == 45)
                    {
                        if ((sobeloutmagB[x, y] <= sobeloutmagB[x - 1, y + 1])
                            || (sobeloutmagB[x, y] <= sobeloutmagB[x + 1, y - 1]))
                        {
                            mag_supB[x, y] = 0;
                        }
                    }
                    else if (sobeloutdirB[x, y] == 90)
                    {
                        if ((sobeloutmagB[x, y] <= sobeloutmagB[x + 1, y])
                            || (sobeloutmagB[x, y] <= sobeloutmagB[x - 1, y]))
                        {
                            mag_supB[x, y] = 0;
                        }
                    }
                    else
                    {
                        if ((sobeloutmagB[x, y] <= sobeloutmagB[x + 1, y + 1])
                            || (sobeloutmagB[x, y] <= sobeloutmagB[x - 1, y - 1]))
                        {
                            mag_supB[x, y] = 0;
                        }
                    }

                }
            }

            double maxR = 0, maxG = 0, maxB = 0;
            for (int i = 0; i < gradxR.Count; i++)
            {
                for (int j = 0; j < gradxR[0].Count; j++)
                {
                    if (mag_supR[i, j] > maxR) maxR = mag_supR[i, j];
                    if (mag_supG[i, j] > maxG) maxG = mag_supG[i, j];
                    if (mag_supB[i, j] > maxB) maxB = mag_supB[i, j];
                }
            }

            double thR = 0.2 * maxR;
            double tlR = 0.1 * maxR;
            double thG = 0.2 * maxG;
            double tlG = 0.1 * maxG;
            double thB = 0.2 * maxB;
            double tlB = 0.1 * maxB;

            double[,] gnhR = new double[width, height];
            double[,] gnlR = new double[width, height];
            double[,] gnhG = new double[width, height];
            double[,] gnlG = new double[width, height];
            double[,] gnhB = new double[width, height];
            double[,] gnlB = new double[width, height];

            for (int i = 0; i < gradxR.Count; i++)
            {
                for (int j = 0; j < gradxR[0].Count; j++)
                {
                    if (mag_supR[i, j] >= thR) gnhR[i, j] = mag_supR[i, j];
                    if (mag_supR[i, j] >= tlR) gnlR[i, j] = mag_supR[i, j];

                    if (mag_supG[i, j] >= thG) gnhG[i, j] = mag_supG[i, j];
                    if (mag_supG[i, j] >= tlG) gnlG[i, j] = mag_supG[i, j];

                    if (mag_supB[i, j] >= thB) gnhB[i, j] = mag_supB[i, j];
                    if (mag_supB[i, j] >= tlB) gnlB[i, j] = mag_supB[i, j];
                }
            }

            for (int i = 0; i < gradxR.Count; i++)
            {
                for (int j = 0; j < gradxR[0].Count; j++)
                {
                    gnlR[i, j] = gnlR[i, j] - gnhR[i, j];
                    gnlG[i, j] = gnlG[i, j] - gnhG[i, j];
                    gnlB[i, j] = gnlB[i, j] - gnhB[i, j];
                }
            }
            ///????
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (gnhR[i, j] > 0)
                    {
                        gnhR[i, j] = 255;
                        traverse(i, j, ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                    }
                    if (gnhG[i, j] > 0)
                    {
                        gnhG[i, j] = 255;
                        traverse(i, j, ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                    }
                    if (gnhB[i, j] > 0)
                    {
                        gnhB[i, j] = 255;
                        traverse(i, j, ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                    }
                }
            }

            for (int x = 0; x < imageSize.X; x++)
            {
                for (int y = 0; y < imageSize.Y; y++)
                {
                    if ((int)gnhR[x, y] > 255) gnhR[x, y] = 255;
                    if ((int)gnhG[x, y] > 255) gnhG[x, y] = 255;
                    if ((int)gnhB[x, y] > 255) gnhB[x, y] = 255;


                    Color oldColor = image.GetPixel(x, y);
                    if (currentChannel == 0 || currentChannel == 4)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb((int)gnhR[x, y], (int)gnhB[x, y], (int)gnhB[x, y]));
                    }
                    else if (currentChannel == 1)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb((int)gnhR[x, y], oldColor.G, oldColor.B));
                    }
                    else if (currentChannel == 2)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb(oldColor.R, (int)gnhB[x, y], oldColor.B));
                    }
                    else if (currentChannel == 3)
                    {
                        rezult.SetPixel(x, y, Color.FromArgb(oldColor.R, oldColor.G, (int)gnhB[x, y]));
                    }
                }
            }


            return rezult;
        }

        //тёмная магия
        private static void traverse(int i, int j, ref double[,] gnhR, ref double[,] gnlR, ref double[,] gnhG, ref double[,] gnlG, ref double[,] gnhB, ref double[,] gnlB)
        {
            if ((i == 0) || (j == 0))
                return;
            int[] x = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] y = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };
            for (int k = 0; k < 8; k++)
            {
                if (gnhR[i + x[k], j + y[k]] == 0 && gnlR[i + x[k], j + y[k]] != 0)
                {
                    gnhR[i + x[k], j + y[k]] = 255;
                    traverse(i + x[k], j + y[k], ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                }
                if (gnhG[i + x[k], j + y[k]] == 0 && gnlG[i + x[k], j + y[k]] != 0)
                {
                    gnhG[i + x[k], j + y[k]] = 255;
                    traverse(i + x[k], j + y[k], ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                }
                if (gnhB[i + x[k], j + y[k]] == 0 && gnlB[i + x[k], j + y[k]] != 0)
                {
                    gnhB[i + x[k], j + y[k]] = 255;
                    traverse(i + x[k], j + y[k], ref gnhR, ref gnlR, ref gnhG, ref gnlG, ref gnhB, ref gnlB);
                }
            }
        }

        public static Bitmap applyCannySegmentation(string colorModel, int currentChannel, Bitmap image, Point imageSize, int gausSize, double sigma, float amplifier = 1, double threshold = 10)
        {
            Bitmap rezult = new Bitmap(image);

            Canny cannyData = new Canny(image, 20F, (float)threshold, gausSize, (float)sigma);

            rezult = cannyData.DisplayImage(cannyData.EdgeMap);

            return rezult;
        }
    }
}

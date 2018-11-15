using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class lab2 : Form
    {
        private Bitmap originalBitmap = null;
        private Bitmap previewBitmap = null;
        private Bitmap resultBitmap = null;
        private Bitmap orig;
        int colorChannel = 0;
        private PictureBox respBox;
        private Bitmap RGBImage, YUVImage;

        public lab2(Bitmap orig, PictureBox pb, Bitmap or)
        {
            InitializeComponent();
            this.orig = orig;
            pictureBox1.Image = orig;
            comboBox1.SelectedIndex = 0;
            originalBitmap = this.orig;
            previewBitmap = originalBitmap.CopyToSquareCanvas(pictureBox1.Width);
            respBox = pb;
            RGBImage = new Bitmap(or);
            YUVImage = new Bitmap(or.Width, or.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Noize n = new Noize(new Bitmap(pictureBox1.Image), ((radioButton1.Checked) ? 0 : 2), pictureBox1);
            n.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox1.Image = orig;
            }
            else
            {
                double Y, u, v;
                Bitmap res = new Bitmap(orig.Width, orig.Height);
                for (int x = 0; x < orig.Width; x++)
                {
                    for (int y = 0; y < orig.Height; y++)
                    {
                        lab1.ColorToYUV(orig.GetPixel(x, y), out Y, out u, out v);
                        res.SetPixel(x, y, Color.FromArgb(lab1.TrimRGB(Y), lab1.TrimRGB(u), lab1.TrimRGB(v)));
                    }
                }
                pictureBox1.Image = res;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox1.Image = orig;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                double Y, u, v;
                Bitmap res = new Bitmap(orig.Width, orig.Height);
                for (int x = 0; x < orig.Width; x++)
                {
                    for (int y = 0; y < orig.Height; y++)
                    {
                        lab1.ColorToYUV(orig.GetPixel(x, y), out Y, out u, out v);
                        res.SetPixel(x, y, Color.FromArgb(lab1.TrimRGB(Y), lab1.TrimRGB(u), lab1.TrimRGB(v)));
                    }
                }
                pictureBox1.Image = res;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap orig = (Bitmap) pictureBox1.Image;
            pictureBox1.Image = applyRecursiveFilter("RGB", orig,
                new Point(orig.Width, orig.Height),
                new Point((int) xSizeNumUpDown.Value, (int) ySizeNumUpDown.Value), colorChannel);
        }

        public Bitmap applyRecursiveFilter(string colorMode, Bitmap image, Point imageSize, Point filterSize,
            int colorChannelIndex = 0)
        {
            Bitmap rezult = new Bitmap(image);

            ApertureService service = new ApertureService();

            List<Aperture> apertures = service.getApertureMatrixGenerator(imageSize, filterSize);

            List<int> redList = new List<int>();
            List<int> greenList = new List<int>();
            List<int> blueList = new List<int>();

            double[] rSumsArray = new double[apertures.Count];
            double[] gSumsArray = new double[apertures.Count];
            double[] bSumsArray = new double[apertures.Count];

            for (int x = 0; x < apertures.Count; x++)
            {
                redList.Clear();
                blueList.Clear();
                greenList.Clear();
                int rSum = 0, gSum = 0, bSum = 0, kSum = 0;
                List<List<Point>> matrix = apertures[x].matrix;
                bool isSideAperture = false;
                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        //проверяем, выйдем ли мы за границы изображения слева, при расчетах, 
                        // ведь идем от -r до +r потом.
                        if (pixelPosX <= filterSize.X || pixelPosY <= filterSize.Y)
                            isSideAperture = true;

                        Color color = image.GetPixel(pixelPosX, pixelPosY);
                        redList.Add(color.R);
                        greenList.Add(color.G);
                        blueList.Add(color.B);
                        kSum += 1;
                    }
                }
                //если при расчетах выйдем за рамки, считаем просто стреднее арифметическое.
                if (isSideAperture)
                {
                    for (int i = 0; i < redList.Count; i++)
                    {
                        rSum += redList[i];
                        gSum += greenList[i];
                        bSum += blueList[i];
                    }

                    rSumsArray[x] = rSum;
                    gSumsArray[x] = gSum;
                    bSumsArray[x] = bSum;
                }
                else
                {
                    int rSum1 = 0, gSum1 = 0, bSum1 = 0, kSum1 = 0;
                    int rSum2 = 0, gSum2 = 0, bSum2 = 0, kSum2 = 0;

                    //крайне правый элемент предыдущего окна
                    for (int i = 0; i < matrix.Count; i++)
                    {
                        Color color = image.GetPixel(matrix[i][0].X - 1, matrix[i][0].Y);
                        rSum1 += color.R;
                        gSum1 += color.G;
                        bSum1 += color.B;
                        kSum1 += 1;
                    }

                    for (int i = 0; i < matrix.Count; i++)
                    {
                        Color color = image.GetPixel(matrix[i][filterSize.X - 1].X, matrix[i][filterSize.X - 1].Y);
                        rSum2 += color.R;
                        gSum2 += color.G;
                        bSum2 += color.B;
                        kSum2 += 1;
                    }

                    //вычислять рекурсию только если предыдущая апертура на той же линии
                    if (apertures[x].y == apertures[x - 1].y)
                    {
                        double rSumReal = rSumsArray[x - 1] - rSum1 + rSum2;
                        double gSumReal = gSumsArray[x - 1] - rSum1 + rSum2;
                        double bSumReal = gSumsArray[x - 1] - rSum1 + rSum2;

                        rSumsArray[x] = rSumReal;
                        gSumsArray[x] = gSumReal;
                        bSumsArray[x] = bSumReal;
                    }
                    else
                    {
                        for (int i = 0; i < redList.Count; i++)
                        {
                            rSum += redList[i];
                            gSum += greenList[i];
                            bSum += blueList[i];
                        }

                        rSumsArray[x] = rSum;
                        gSumsArray[x] = gSum;
                        bSumsArray[x] = bSum;
                    }
                }

                // int pixelCount = filterSize.X * filterSize.Y;

                rSum = (int) rSumsArray[x] / kSum;
                if (rSum < 0)
                    rSum = 0;
                else if (rSum > 255)
                    rSum = 255;

                gSum = (int) gSumsArray[x] / kSum;
                if (gSum < 0)
                    gSum = 0;
                else if (gSum > 255)
                    gSum = 255;

                bSum = (int) bSumsArray[x] / kSum;
                if (bSum < 0)
                    bSum = 0;
                else if (bSum > 255)
                    bSum = 255;

                int aperturePosX = matrix.Count / 2;
                if (matrix.Count != 0 && matrix[aperturePosX].Count / 2 != 0)
                {
                    int aperturePosY = matrix[aperturePosX].Count / 2;

                    Color oldColor = image.GetPixel(matrix[aperturePosX][aperturePosY].X,
                        matrix[aperturePosX][aperturePosY].Y);

                    if (colorChannelIndex == 0)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                            Color.FromArgb((int) rSum, (int) gSum, (int) bSum));
                    }
                    else if (colorChannelIndex == 1)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                            Color.FromArgb((int) rSum, oldColor.G, oldColor.B));
                    }
                    else if (colorChannelIndex == 2)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                            Color.FromArgb(oldColor.R, (int) gSum, oldColor.B));
                    }
                    else if (colorChannelIndex == 3)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                            Color.FromArgb(oldColor.R, oldColor.G, (int) bSum));
                    }
                }
            }

            service = null;
            for (int i = 0; i < apertures.Count; i++)
            {
                apertures[i].Dispose();
            }
            apertures.Clear();

            return rezult;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            colorChannel = 0;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            colorChannel = 1;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            colorChannel = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            colorChannel = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap orig = (Bitmap) pictureBox1.Image;
            pictureBox1.Image = applyMedianFilter("RGB", orig,
                new Point(orig.Width, orig.Height),
                new Point((int) xSizeNumUpDown.Value + 1, (int) ySizeNumUpDown.Value + 1), colorChannel);
        }

        public Bitmap applyMedianFilter(string colorMode, Bitmap image, Point imageSize, Point filterSize,
            int colorChannelIndex = 0)
        {
            Bitmap rezult = new Bitmap(image);

            ApertureService service = new ApertureService();

            List<Aperture> apertures = service.getApertureMatrixGenerator(imageSize, filterSize);

            List<int> redList = new List<int>();
            List<int> greenList = new List<int>();
            List<int> blueList = new List<int>();

            for (int x = 0; x < apertures.Count; x++)
            {
                redList.Clear();
                blueList.Clear();
                greenList.Clear();

                List<List<Point>> matrix = apertures[x].matrix;
                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        Color color = image.GetPixel(pixelPosX, pixelPosY);
                        redList.Add(color.R);
                        greenList.Add(color.G);
                        blueList.Add(color.B);
                    }
                }

                redList.Sort();
                greenList.Sort();
                blueList.Sort();

                int apertureCenter = redList.Count / 2;
                int rValue = redList[apertureCenter];
                int gValue = greenList[apertureCenter];
                int bValue = blueList[apertureCenter];

                int aperturePosX = matrix.Count / 2;
                int aperturePosY = matrix[aperturePosX].Count / 2;

                Color oldColor = image.GetPixel(matrix[aperturePosX][aperturePosY].X,
                    matrix[aperturePosX][aperturePosY].Y);

                if (colorChannelIndex == 0)
                {
                    rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                        Color.FromArgb(rValue, gValue, bValue));
                }
                else if (colorChannelIndex == 1)
                {
                    rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                        Color.FromArgb(rValue, oldColor.G, oldColor.B));
                }
                else if (colorChannelIndex == 2)
                {
                    rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                        Color.FromArgb(oldColor.R, gValue, oldColor.B));
                }
                else if (colorChannelIndex == 3)
                {
                    rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y,
                        Color.FromArgb(oldColor.R, oldColor.G, bValue));
                }
            }

            service = null;
            for (int i = 0; i < apertures.Count; i++)
            {
                apertures[i].Dispose();
            }
            apertures.Clear();

            return rezult;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap orig = (Bitmap) pictureBox1.Image;
            pictureBox1.Image = applyAdaptiveFilter(orig,
                new Point(orig.Width, orig.Height),
                new Point((int) xSizeNumUpDown.Value, (int) ySizeNumUpDown.Value), colorChannel);
        }

        public Bitmap applyAdaptiveFilter(Bitmap image, Point imageSize, Point maxFilterSize, int colorChannel = 0)
        {
            Bitmap rezult = new Bitmap(image);

            int minXFilterSize = 2;
            int minYFilterSize = 2;

            int xFilterSize, yFilterSize;

            for (int x = 0; x < imageSize.X; x++)
            {
                for (int y = 0; y < imageSize.Y; y++)
                {
                    xFilterSize = minXFilterSize;
                    yFilterSize = minYFilterSize;
                    Color oldColor = image.GetPixel(x, y);
                    int R, G, B;
                    if (colorChannel == 0)
                    {
                        R = adaptiveFilterProcess(image, "R", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                        G = adaptiveFilterProcess(image, "G", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                        B = adaptiveFilterProcess(image, "B", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                    }
                    else if (colorChannel == 1)
                    {
                        R = adaptiveFilterProcess(image, "R", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                        G = oldColor.G;
                        B = oldColor.B;
                    }
                    else if (colorChannel == 2)
                    {
                        R = oldColor.R;
                        G = adaptiveFilterProcess(image, "G", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                        B = oldColor.B;
                    }
                    else if (colorChannel == 3)
                    {
                        R = oldColor.R;
                        G = oldColor.G;
                        B = adaptiveFilterProcess(image, "B", x, y, new Point(xFilterSize, yFilterSize), imageSize,
                            maxFilterSize);
                    }
                    else
                    {
                        R = oldColor.R;
                        G = oldColor.G;
                        B = oldColor.B;
                    }

                    Color color = Color.FromArgb(R, G, B);
                    rezult.SetPixel(x, y, color);
                }
            }

            return rezult;
        }

        public int adaptiveFilterProcess(Bitmap image, string colorMode, int x, int y, Point filterSize, Point imageSize,
            Point maxFilterSize)
        {
            ApertureService service = new ApertureService();
            int pixelPosX = 0, pixelPosY = 0;
            int rMax = 0, gMax = 0, bMax = 0;
            int rMin = 256, gMin = 256, bMin = 256;
            int rMedian, gMedian, bMedian;
            Color thisPixelColor = image.GetPixel(x, y);

            List<int> redList = new List<int>();
            List<int> greenList = new List<int>();
            List<int> blueList = new List<int>();


            for (int i = 0; i < filterSize.X; i++)
            {
                for (int j = 0; j < filterSize.Y; j++)
                {
                    service.getAperturePosition(x, y, imageSize, i, j, new Point(filterSize.X, filterSize.Y),
                        out pixelPosX, out pixelPosY);

                    if (pixelPosX == -1 || pixelPosY == -1)
                        continue;
                    else
                    {
                        Color color = image.GetPixel(pixelPosX, pixelPosY);

                        if (rMax < color.R)
                            rMax = color.R;
                        if (rMin > color.R)
                            rMin = color.R;

                        if (gMax < color.G)
                            gMax = color.G;
                        if (gMin > color.G)
                            gMin = color.G;

                        if (bMax < color.B)
                            bMax = color.B;
                        if (bMin > color.B)
                            bMin = color.B;

                        redList.Add(color.R);
                        greenList.Add(color.G);
                        blueList.Add(color.B);
                    }
                }
            }

            //сортировка списков
            redList.Sort();
            greenList.Sort();
            blueList.Sort();

            //находим медиану яркости для апертуры
            int apertureCenter = redList.Count / 2;
            rMedian = redList[apertureCenter];
            gMedian = greenList[apertureCenter];
            bMedian = blueList[apertureCenter];

            int checkMedian, checkMin, checkMax, checkColor;

            if (colorMode == "R")
            {
                checkMedian = rMedian;
                checkMin = rMin;
                checkMax = rMax;
                checkColor = thisPixelColor.R;
            }
            else if (colorMode == "G")
            {
                checkMedian = gMedian;
                checkMin = gMin;
                checkMax = gMax;
                checkColor = thisPixelColor.G;
            }
            else if (colorMode == "B")
            {
                checkMedian = bMedian;
                checkMin = bMin;
                checkMax = bMax;
                checkColor = thisPixelColor.B;
            }
            else
                return 0;

            //branch a
            if (checkMedian - checkMin > 0 && checkMedian - checkMax < 0) //goto branch b
            {
                //branch b
                if (checkColor - checkMin > 0 && checkColor - checkMax < 0)
                {
                    return checkColor;
                }
                else
                {
                    return checkMedian;
                }
            }
            else
            {
                filterSize.X++;
                filterSize.Y++;

                if (filterSize.X > maxFilterSize.X || filterSize.Y > maxFilterSize.Y)
                {
                    return checkColor;
                }
                else
                {
                    return adaptiveFilterProcess(image, colorMode, x, y, filterSize, imageSize, maxFilterSize);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap previewBitmap = (Bitmap) pictureBox1.Image;
            if (previewBitmap == null || comboBox1.SelectedIndex == -1)
            {
                return;
            }

            Bitmap selectedSource = null;
            Bitmap bitmapResult = null;

            selectedSource = (Bitmap) pictureBox1.Image;


            float factor = trackBar1.Value / 100.0f;
            if (comboBox1.SelectedItem.ToString() == "None")
            {
                bitmapResult = previewBitmap;
            }
            else if (comboBox1.SelectedItem.ToString() == "Gaussian 3x3")
            {
                bitmapResult = selectedSource.UnsharpGaussian3x3(factor);
            }
            else if (comboBox1.SelectedItem.ToString() == "Gaussian 5x5")
            {
                bitmapResult = selectedSource.UnsharpGaussian5x5(factor);
            }
            else if (comboBox1.SelectedItem.ToString() == "Mean 3x3")
            {
                bitmapResult = selectedSource.UnsharpMean3x3(factor);
            }
            else if (comboBox1.SelectedItem.ToString() == "Mean 5x5")
            {
                bitmapResult = selectedSource.UnsharpMean5x5(factor);
            }

            pictureBox1.Image = bitmapResult;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Интенсивность:" + trackBar1.Value.ToString() + " %";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = KuwaharaBlur((Bitmap) pictureBox1.Image, (int) numericUpDown1.Value);
        }

        public static Bitmap KuwaharaBlur(Bitmap Image, int Size)
        {
            Bitmap TempBitmap = Image;
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height),
                new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            int[] ApetureMinX = {-(Size / 2), 0, -(Size / 2), 0};
            int[] ApetureMaxX = {0, (Size / 2), 0, (Size / 2)};
            int[] ApetureMinY = {-(Size / 2), -(Size / 2), 0, 0};
            int[] ApetureMaxY = {0, 0, (Size / 2), (Size / 2)};
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int[] RValues = {0, 0, 0, 0};
                    int[] GValues = {0, 0, 0, 0};
                    int[] BValues = {0, 0, 0, 0};
                    int[] NumPixels = {0, 0, 0, 0};
                    int[] MaxRValue = {0, 0, 0, 0};
                    int[] MaxGValue = {0, 0, 0, 0};
                    int[] MaxBValue = {0, 0, 0, 0};
                    int[] MinRValue = {255, 255, 255, 255};
                    int[] MinGValue = {255, 255, 255, 255};
                    int[] MinBValue = {255, 255, 255, 255};
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < NewBitmap.Width)
                            {
                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < NewBitmap.Height)
                                    {
                                        Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                        {
                                            MaxRValue[i] = TempColor.R;
                                        }
                                        else if (TempColor.R < MinRValue[i])
                                        {
                                            MinRValue[i] = TempColor.R;
                                        }

                                        if (TempColor.G > MaxGValue[i])
                                        {
                                            MaxGValue[i] = TempColor.G;
                                        }
                                        else if (TempColor.G < MinGValue[i])
                                        {
                                            MinGValue[i] = TempColor.G;
                                        }

                                        if (TempColor.B > MaxBValue[i])
                                        {
                                            MaxBValue[i] = TempColor.B;
                                        }
                                        else if (TempColor.B < MinBValue[i])
                                        {
                                            MinBValue[i] = TempColor.B;
                                        }
                                        ++NumPixels[i];
                                    }
                                }
                            }
                        }
                    }
                    int j = 0;
                    int MinDifference = 10000;
                    for (int i = 0; i < 4; ++i)
                    {
                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) +
                                                (MaxBValue[i] - MinBValue[i]);
                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                        {
                            j = i;
                            MinDifference = CurrentDifference;
                        }
                    }

                    Color MeanPixel = Color.FromArgb(RValues[j] / NumPixels[j],
                        GValues[j] / NumPixels[j],
                        BValues[j] / NumPixels[j]);
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return NewBitmap;
        }

        private void вернутьВОсноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            respBox.Image = pictureBox1.Image;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            double mseR = 0, mseG = 0, mseB = 0, mseInt = 0;
            double psnrR = 0, psnrG = 0, psnrB = 0, psnrInt = 0;


            Bitmap compareImage;
            if (comboBox1.SelectedIndex == 0)
            {
                compareImage = new Bitmap(RGBImage);
            }
            else
            {
                compareImage = new Bitmap(YUVImage);
            }

            calculateMSE(compareImage, (Bitmap)pictureBox1.Image, out mseR, out mseG, out mseB, out mseInt);
            calculatePSNR(compareImage, (Bitmap)pictureBox1.Image, out psnrR, out psnrG, out psnrB, out psnrInt);

            MSERLabel.Text = "Difference R: " + Math.Round(mseR, 3);
            MSEGLabel.Text = "Difference G: " + Math.Round(mseG, 3);
            MSEBLabel.Text = "Difference B: " + Math.Round(mseB, 3);
            MSEIntLabel.Text = "Difference Int: " + Math.Round(mseInt, 3);

            PSNRRLabel.Text = "Difference R: " + Math.Round(psnrR, 3);
            PSNRGLabel.Text = "Difference G: " + Math.Round(psnrG, 3);
            PSNRBLabel.Text = "Difference B: " + Math.Round(psnrB, 3);
            PSNRIntLabel.Text = "Difference Int: " + Math.Round(psnrInt, 3);
        }

        public void calculateMSE(Bitmap originalImage, Bitmap resultImage, out double MSEResultR, out double MSEResultG,
            out double MSEResultB, out double MSEResultInt)
        {
            MSEResultR = 0;
            MSEResultG = 0;
            MSEResultB = 0;
            MSEResultInt = 0;

            if (originalImage == null || resultImage == null)
                return;


            int originalImagePixels = originalImage.Width * originalImage.Height;
            int resultImagePixels = resultImage.Width * resultImage.Height;

            int[] originalR, originalG, originalB, originalIntColor;
            int[] resultR, resultG, resultB, resultIntColor;

            originalR = new int[originalImagePixels];
            originalG = new int[originalImagePixels];
            originalB = new int[originalImagePixels];

            originalIntColor = new int[originalImagePixels];

            resultR = new int[resultImagePixels];
            resultG = new int[resultImagePixels];
            resultB = new int[resultImagePixels];

            resultIntColor = new int[resultImagePixels];

            //сбор сумм всех цветовых каналов
            for (int x = 0; x < originalImage.Width; x++)
            for (int y = 0; y < originalImage.Height; y++)
            {
                Color color = originalImage.GetPixel(x, y);
                int arrayLocation = originalImage.Height * x + y;

                originalR[arrayLocation] = color.R;
                originalG[arrayLocation] = color.G;
                originalB[arrayLocation] = color.B;

                originalIntColor[arrayLocation] = color.ToArgb();
            }

            for (int x = 0; x < resultImage.Width; x++)
            for (int y = 0; y < resultImage.Height; y++)
            {
                Color color = resultImage.GetPixel(x, y);
                int arrayLocation = resultImage.Height * x + y;

                resultR[arrayLocation] = color.R;
                resultG[arrayLocation] = color.G;
                resultB[arrayLocation] = color.B;

                resultIntColor[arrayLocation] = color.ToArgb();
            }

            double MSESumR = 0, MSESumG = 0, MSESumB = 0, MSESumInt = 0;

            for (int x = 0; x < originalR.Length; x++)
            {
                MSESumR += Math.Pow((originalR[x] - resultR[x]), 2);
                MSESumG += Math.Pow((originalG[x] - resultG[x]), 2);
                MSESumB += Math.Pow((originalB[x] - resultB[x]), 2);

                MSESumInt += Math.Pow((originalIntColor[x] - resultIntColor[x]), 2);
            }

            //MSE - средняя арифметическая
            MSEResultR = MSESumR / originalR.Length;
            MSEResultG = MSESumG / originalG.Length;
            MSEResultB = MSESumB / originalB.Length;
            MSEResultInt = MSESumInt / originalIntColor.Length;
        }

        public void calculatePSNR(Bitmap originalImage, Bitmap resultImage, out double PSNRResultR,
            out double PSNRResultG, out double PSNRResultB, out double PSNRResultInt)
        {
            double MSEResultR = 0, MSEResultG = 0, MSEResultB = 0, MSEResultInt = 0;

            PSNRResultR = 0;
            PSNRResultG = 0;
            PSNRResultB = 0;
            PSNRResultInt = 0;

            if (originalImage == null || resultImage == null)
                return;

            calculateMSE(originalImage, resultImage, out MSEResultR, out MSEResultG, out MSEResultB, out MSEResultInt);

            if (MSEResultR == 0)
                PSNRResultR = 0;
            else
            {
                PSNRResultR = 20 * Math.Log10(255 / (double) Math.Sqrt(MSEResultR));
            }

            if (MSEResultG == 0)
                PSNRResultG = 0;
            else
            {
                PSNRResultG = 20 * Math.Log10(255 / (double) Math.Sqrt(MSEResultG));
            }

            if (MSEResultB == 0)
                PSNRResultB = 0;
            else
            {
                PSNRResultB = 20 * Math.Log10(255 / (double) Math.Sqrt(MSEResultB));
            }

            if (MSEResultInt == 0)
                PSNRResultInt = 0;
            else
            {
                PSNRResultInt = 20 * Math.Log10(255 / (double) Math.Sqrt(MSEResultInt));
            }
        }
    }
}
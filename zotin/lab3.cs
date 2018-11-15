using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class lab3 : Form
    {
        private Bitmap originIMG;
        private readonly ImageProcessor _imageProcessor;
        private Image _resultImage;
        int colorIndex = 0;
        private PictureBox resBox;
        public lab3(Bitmap orig, PictureBox pb)
        {
            InitializeComponent();
            originIMG = orig;
            _imageProcessor = new ImageProcessor();
            resBox = pb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int realThreshold = 0;
            pictureBox1.Image = applyOtzuThersholding((Bitmap)pictureBox1.Image,
                new Point(pictureBox1.Image.Width, pictureBox1.Image.Height), 
                (int)numericUpDown1.Value, out realThreshold);
        }
        public Bitmap applyOtzuThersholding(Bitmap image, Point imageSize, int k, out int realThreshold, bool convert = true)
        {
            Bitmap rezult = new Bitmap(image);

            Otzu.ApplyOtsuThreshold(ref rezult, out realThreshold, k);
            return rezult;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = applyBernzenThersholding((Bitmap)pictureBox1.Image,
                                        new Point(pictureBox1.Image.Width, pictureBox1.Image.Height),
                                        new Point((int)numericUpDownX.Value, (int)numericUpDownY.Value), checkBox1.Checked);
        }

        public Bitmap convertImageToGrayscale(Bitmap image)
        {
            Bitmap result = new Bitmap(image);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    int grayscale = (int)((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));

                    result.SetPixel(i, j, Color.FromArgb(grayscale, grayscale, grayscale));
                }
            }
            return result;
        }

        public Bitmap applyBernzenThersholding(Bitmap image, Point imageSize, Point filterSize, bool convert = true)
        {
            Bitmap rezult = new Bitmap(image);
            Bitmap tempImage;
            if (convert)
                tempImage = convertImageToGrayscale(image);
            else
                tempImage = new Bitmap(image);

            ApertureService service = new ApertureService();

            List<Aperture> apertures = service.getApertureMatrixGenerator(imageSize, filterSize);

            for (int x = 0; x < apertures.Count; x++)
            {
                int minValue = 255, maxValue = 0;
                double average;

                List<List<Point>> matrix = apertures[x].matrix;
                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        //т.к. картинка черно белая, то значения РГБ одинаковы, можно брыть любое из трех.
                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R > maxValue)
                            maxValue = tempImage.GetPixel(pixelPosX, pixelPosY).R;
                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R < minValue)
                            minValue = tempImage.GetPixel(pixelPosX, pixelPosY).R;
                    }
                }
                average = ((double)minValue + maxValue) / 2;

                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;


                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R < average)
                            rezult.SetPixel(pixelPosX, pixelPosY, Color.FromArgb(0, 0, 0));
                        else
                            rezult.SetPixel(pixelPosX, pixelPosY, Color.FromArgb(255, 255, 255));
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

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = applyNiblackThersholding((Bitmap)pictureBox1.Image,
                            new Point(pictureBox1.Image.Width, pictureBox1.Image.Height),
                            new Point((int)numericUpDownX.Value, (int)numericUpDownY.Value), 0.2, checkBox1.Checked);
        }

        public Bitmap applyNiblackThersholding(Bitmap image, Point imageSize, Point filterSize, double k = 0.2, bool convert = true)
        {
            Bitmap rezult = new Bitmap(image);
            Bitmap tempImage;
            if (convert)
                tempImage = convertImageToGrayscale(image);
            else
                tempImage = new Bitmap(image);

            ApertureService service = new ApertureService();

            List<Aperture> apertures = service.getApertureMatrixGenerator(imageSize, filterSize);

            for (int x = 0; x < apertures.Count; x++)
            {
                int minValue = 255, maxValue = 0;
                double average, RMS, differenceSum, value;

                List<List<Point>> matrix = apertures[x].matrix;
                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        //т.к. картинка черно белая, то значения РГБ одинаковы, можно брыть любое из трех.
                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R > maxValue)
                            maxValue = tempImage.GetPixel(pixelPosX, pixelPosY).R;
                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R < minValue)
                            minValue = tempImage.GetPixel(pixelPosX, pixelPosY).R;
                    }
                }
                average = ((double)minValue + maxValue) / 2;
                RMS = 0;
                differenceSum = 0;

                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;
                        differenceSum = Math.Pow((tempImage.GetPixel(pixelPosX, pixelPosY).R - average), 2);
                    }
                }

                RMS += Math.Sqrt(differenceSum / (filterSize.X * filterSize.Y));
                value = (int)(average + k * RMS);

                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        if (tempImage.GetPixel(pixelPosX, pixelPosY).R < value)
                            rezult.SetPixel(pixelPosX, pixelPosY, Color.FromArgb(0, 0, 0));
                        else
                            rezult.SetPixel(pixelPosX, pixelPosY, Color.FromArgb(255, 255, 255));
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

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = originIMG;
        }

        private void SegmentationMorphologization(object sender, EventArgs e)
        {
            _imageProcessor.Open((Bitmap)pictureBox1.Image, true);
            if (!IsResultImageSet) return;
            var radius = trackBarSegmentationMorphologizationRadius.Value;
            var structureElement = comboBoxSegmentationMorphologizationStructureElement.SelectedIndex;

            Bitmap result;
            if (radioButtonSegmentationMorphologizationDelation.Checked)
            {
                result = _imageProcessor.SegmentationMorphologizationDilation(radius, structureElement);
            }
            else
            {
                result = _imageProcessor.SegmentationMorphologizationErosion(radius, structureElement);
            }
            SetResultImage(result);
        }
        private void SetResultImage(Image image)
        {
            _resultImage = image;
            pictureBox1.Image = image;
        }

        private bool IsResultImageSet
        {
            get { return pictureBox1.Image != null; }
        }

        private void lab3_Shown(object sender, EventArgs e)
        {
            pictureBox1.Image = originIMG;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            if (colorIndex == 4)
            {
                image = convertImageToGrayscale((Bitmap) pictureBox1.Image);
            }
            pictureBox1.Image = Segmentation.applyRobertsSegmentation("RGB", colorIndex, image,
                    new Point(pictureBox1.Image.Width, pictureBox1.Image.Height),
                    (float)numericUpDownAmp.Value, (double)numericUpDownRobertsThreshold.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            if (colorIndex == 4)
            {
                image = convertImageToGrayscale((Bitmap) pictureBox1.Image);
            }

            pictureBox1.Image = Segmentation.applySobelSegmentation("RGB", colorIndex, image,
                    new Point(pictureBox1.Image.Width, pictureBox1.Image.Height),
                    (float)numericUpDownAmp.Value, (double)numericUpDownRobertsThreshold.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            if (colorIndex == 4)
            {
                image = convertImageToGrayscale((Bitmap) pictureBox1.Image);
            }

            Bitmap gauss = applyGaussianBlurFilter("RGB", (Bitmap)pictureBox1.Image, new Point(pictureBox1.Image.Width, pictureBox1.Image.Height), new Point(3, 3));

            pictureBox1.Image = Segmentation.applyCannySegmentation2("RGB", colorIndex, image,
                new Point(pictureBox1.Image.Width, pictureBox1.Image.Height), gauss);
        }

        public static Bitmap applyGaussianBlurFilter(string colorMode, Bitmap image, Point imageSize, Point filterSize, int colorChannelIndex = 0)
        {
            Bitmap rezult = new Bitmap(image);

            ApertureService service = new ApertureService();

            List<Aperture> apertures = service.getApertureMatrixGenerator(imageSize, filterSize);

            double sigma = 0.5;

            for (int x = 0; x < apertures.Count; x++)
            {
                double rSum = 0, gSum = 0, bSum = 0, kSum = 0;

                List<List<Point>> matrix = apertures[x].matrix;
                int line = 0;
                foreach (var matrixLine in matrix)
                {
                    foreach (var Point in matrixLine)
                    {
                        int pixelPosX = Point.X;
                        int pixelPosY = Point.Y;

                        Color color = image.GetPixel(pixelPosX, pixelPosY);
                        double kVal = calculateGaussian(line, sigma);

                        rSum += color.R * kVal;
                        gSum += color.G * kVal;
                        bSum += color.B * kVal;

                        kSum += kVal;
                    }
                    line++;
                }

                if (kSum <= 0)
                    kSum = 1;

                rSum = rSum / kSum;
                if (rSum < 0)
                    rSum = 0;
                else if (rSum > 255)
                    rSum = 255;

                gSum = gSum / kSum;
                if (gSum < 0)
                    gSum = 0;
                else if (gSum > 255)
                    gSum = 255;

                bSum = bSum / kSum;
                if (bSum < 0)
                    bSum = 0;
                else if (bSum > 255)
                    bSum = 255;



                int aperturePosX = matrix.Count / 2;


                if (matrix.Count != 0 && matrix[aperturePosX].Count / 2 != 0)
                {
                    int aperturePosY = matrix[aperturePosX].Count / 2;

                    Color oldColor = image.GetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y);

                    if (colorChannelIndex == 0)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y, Color.FromArgb((int)rSum, (int)gSum, (int)bSum));
                    }
                    else if (colorChannelIndex == 1)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y, Color.FromArgb((int)rSum, oldColor.G, oldColor.B));
                    }
                    else if (colorChannelIndex == 2)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y, Color.FromArgb(oldColor.R, (int)gSum, oldColor.B));
                    }
                    else if (colorChannelIndex == 3)
                    {
                        rezult.SetPixel(matrix[aperturePosX][aperturePosY].X, matrix[aperturePosX][aperturePosY].Y, Color.FromArgb(oldColor.R, oldColor.G, (int)bSum));
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

        public static double calculateGaussian(int x, double sigma)
        {
            return (1 / (Math.Sqrt(2 * Math.PI) * sigma)) * Math.Exp(-1 * (Math.Pow(x, 2) / (2 * Math.Pow(sigma, 2))));
        }

        private void radioButtonRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRGB.Checked)
                colorIndex = 0;
        }

        private void radioButtonR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonR.Checked)
                colorIndex = 1;
        }

        private void radioButtonG_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonG.Checked)
                colorIndex = 2;
        }

        private void radioButtonB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonB.Checked)
                colorIndex = 3;
        }

        private void radioButtonGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGrayscale.Checked)
                colorIndex = 4;
        }

        private void вернутьИзображениеНаГлавнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resBox.Image = pictureBox1.Image;
        }
    }
}

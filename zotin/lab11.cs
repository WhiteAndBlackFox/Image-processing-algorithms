using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class lab11 : Form
    {
        private readonly ImageProcessor _imageProcessor;
        private Image _resultImage;
        public Bitmap imgOrig;
        private PictureBox resPB;
        public lab11(PictureBox pb)
        {
            InitializeComponent();
            _imageProcessor = new ImageProcessor();
            resPB = pb;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sections s = new Sections((Bitmap)pictureBox1.Image, ColorModel.SelectedIndex, pictureBox1);
            s.ShowDialog();
        }

        public Bitmap AlgorithmGrayScale(Func<byte, byte, byte, byte> grayScaleType)
        {
            return ImageProcessor.ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;

                        byte grayColor = grayScaleType(buffer[pixelIndex + 2], buffer[pixelIndex + 1],
                            buffer[pixelIndex]);
                        buffer[pixelIndex + 2] = buffer[pixelIndex + 1] = buffer[pixelIndex] = grayColor;
                    }
                }
            });
        }
        private void SetResultImage(Image image)
        {
            _resultImage = image;
            pictureBox1.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (ColorModel.SelectedIndex)
            {
                case (0):
                {

                    SetResultImage(_imageProcessor.AlgorithmGrayScale(GrayScale.FromRgb));
                    break;
                }
                case (1):
                {
                    SetResultImage(_imageProcessor.AlgorithmGrayScale(GrayScale.FromHsv));
                    break;
                }
                case (2):
                {
                    SetResultImage(_imageProcessor.AlgorithmGrayScale(GrayScale.FromYuv));
                    break;
                }
            }
        }

        private void lab11_Shown(object sender, EventArgs e)
        {
            pictureBox1.Image = imgOrig;
            _imageProcessor.Open(imgOrig, true);
        }

        #region Visualization Color Models
        private void VisualizationRgbModel(object sender, EventArgs e)
        {
            byte r = (byte)(checkBoxRgbR.Checked ? trackBarRgbR.Value : 0);
            byte g = (byte)(checkBoxRgbG.Checked ? trackBarRgbG.Value : 0);
            byte b = (byte)(checkBoxRgbB.Checked ? trackBarRgbB.Value : 0);

            double h, s, v, y, u;
            ClassColorModel.ToHsv(r, g, b, out h, out s, out v);
            trackBarHsvH.Value = GetInt(h);
            trackBarHsvS.Value = GetInt(s * 100);
            trackBarHsvV.Value = GetInt(v * 100);

            ClassColorModel.ToYuv(r, g, b, out y, out u, out v);
            trackBarYuvY.Value = GetInt(y);
            trackBarYuvU.Value = GetInt(u);
            trackBarYuvV.Value = GetInt(v);

            if (sender != null)
            {
                SetResultImage(_imageProcessor.VisualizeRgb(r, g, b));
            }
        }

        private void VisualizationHsvModel(object sender, EventArgs e)
        {
            double h = checkBoxHsvH.Checked ? trackBarHsvH.Value : 0;
            double s = checkBoxHsvS.Checked ? trackBarHsvS.Value / 100f : 0;
            double v = checkBoxHsvV.Checked ? trackBarHsvV.Value / 100f : 0;
            byte r, g, b;
            ClassColorModel.FromHsv(h, s, v, out r, out g, out b);
            trackBarRgbR.Value = r;
            trackBarRgbG.Value = g;
            trackBarRgbB.Value = b;

            double y, u;
            ClassColorModel.ToYuv(r, g, b, out y, out u, out v);
            trackBarYuvY.Value = GetInt(y);
            trackBarYuvU.Value = GetInt(u);
            trackBarYuvV.Value = GetInt(v);

            SetResultImage(_imageProcessor.VisualizeRgb(r, g, b));
        }

        private void VisualizationYuvModel(object sender, EventArgs e)
        {
            double y = checkBoxYuvY.Checked ? trackBarYuvY.Value : 0;
            double u = checkBoxYuvU.Checked ? trackBarYuvU.Value : 0;
            double v = checkBoxYuvV.Checked ? trackBarYuvV.Value : 0;
            byte r, g, b;
            ClassColorModel.FromYuv(y, u, v, out r, out g, out b);
            trackBarRgbR.Value = r;
            trackBarRgbG.Value = g;
            trackBarRgbB.Value = b;

            double h, s;
            ClassColorModel.ToHsv(r, g, b, out h, out s, out v);
            trackBarHsvH.Value = GetInt(h);
            trackBarHsvS.Value = GetInt(s * 100);
            trackBarHsvV.Value = GetInt(v * 100);

            SetResultImage(_imageProcessor.VisualizeRgb(r, g, b));
        }

        #endregion

        #region Help methods
        private int GetInt(double value)
        {
            return (int)Math.Round(value);
        }

        private bool IsResultImageSet
        {
            get { return pictureBox1.Image != null; }
        }

        //Generate Grid sizeM x sizeN
        void GenerateGrid(DataTable table, DataGridView dgv, int sizeM, int sizeN)
        {
            if (table.Rows.Count == sizeM && table.Columns.Count == sizeN) return;
            table.Rows.Clear();
            table.Columns.Clear();
            for (int i = 0; i < sizeN; i++)
            {
                table.Columns.Add("", typeof(double));
            }
            for (int i = 0; i < sizeM; i++)
            {
                var row = table.NewRow();
                table.Rows.Add(row);
            }
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        //fill the grid by matrix
        void FillGrid(DataTable table, double[][] matrix)
        {
            for (var row = 0; row < matrix.Length; row++)
            {
                for (var col = 0; col < matrix[row].Length; col++)
                {
                    table.Rows[row][col] = matrix[row][col].ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        //read matrix from the DataGridView
        double[][] ReadMatrix(DataTable dt)
        {
            int m = 0;
            int n = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    Convert.ToDouble(dt.Rows[i][0]);
                    m++;
                }
                catch (Exception)
                {
                }
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                try
                {
                    Convert.ToDouble(dt.Rows[0][i]);
                    n++;
                }
                catch (Exception)
                {
                }
            }
            var matrix = new double[m][];
            try
            {
                for (int i = 0; i < m; i++)
                {
                    matrix[i] = new double[n];
                    for (int j = 0; j < n; j++)
                    {
                        matrix[i][j] = Convert.ToDouble(dt.Rows[i][j]);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Некорректно заполнены данные матрицы");
            }
            return matrix;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            double[] R, G, B;
            string rName, gName, bName;
            CountHystograms(ColorModel.SelectedIndex, out R, out G, out B, out rName, out gName, out bName, (Bitmap)pictureBox1.Image);

            HystogramForm hf = new HystogramForm(R, G, B, rName, gName, bName, this.Text);
            hf.Show();
        }

        public void CountHystograms(int mode, out double[] R, out double[] G, out double[] B, out string rName,
            out string gName, out string bName, Bitmap orig)
        {
            R = new double[256];
            G = new double[256];
            B = new double[256];
            rName = "Red";
            gName = "Green";
            bName = "Blue";
            Color color;
            for (int i = 0; i < orig.Width; i++)
            {
                for (int j = 0; j < orig.Height; j++)
                {
                    color = orig.GetPixel(i, j);
                    switch (mode)
                    {
                        case 0:
                            {
                                rName = "Красный";
                                gName = "Зеленый";
                                bName = "Синий";
                                ++R[color.R];
                                ++G[color.G];
                                ++B[color.B];
                                break;
                            }
                        case 2:
                            {
                                double H, S, V;
                                lab1.ColorToHSV(color, out H, out S, out V);
                                rName = "Оттенок";
                                gName = "Насыщенность";
                                bName = "Яркость";
                                ++R[(int)(H / 360 * 255)];
                                ++G[(int)(S * 255)];
                                ++B[(int)(V * 255)];
                                break;
                            }
                        case 1:
                            {
                                double Y, U, V;
                                lab1.ColorToYUV(color, out Y, out U, out V);
                                rName = "Y";
                                gName = "U";
                                bName = "V";
                                ++R[(int)Y];
                                ++G[(int)U];
                                ++B[(int)V];
                                break;
                            }
                        case 3:
                            {
                                double H, S, L;
                                lab1.ColorToHSL(color, out H, out S, out L);
                                rName = "Оттенок";
                                gName = "Насыщенность";
                                bName = "Яркость";
                                ++R[(int)(H * 255)];
                                ++G[(int)(S * 255)];
                                ++B[(int)(L * 255)];
                                break;
                            }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CorrecrionImage ci = new CorrecrionImage((Bitmap)pictureBox1.Image, pictureBox1);
            ci.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Noize n = new Noize((Bitmap)pictureBox1.Image, ColorModel.SelectedIndex, pictureBox1);
            n.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _imageProcessor.Open(pictureBox1.Image, false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = imgOrig;
            _imageProcessor.Open(imgOrig, true);
        }

        private void вернутьИзображениеНаГлавнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resPB.Image = pictureBox1.Image;
        }

    }
}

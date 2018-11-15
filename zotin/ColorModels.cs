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
    public partial class ColorModels : Form
    {
        private int idx;
        private Bitmap orig, _bitmap;

        public ColorModels(String name, Bitmap origenal, int indx)
        {
            InitializeComponent();

            this.Text = name;
            pictureBox1.Image = origenal;
            orig = origenal;
            idx = indx;
        }

        private void ColorModels_Shown(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorBalance cm = new ColorBalance(pictureBox1, new Bitmap(pictureBox1.Image), idx);
            cm.ShowDialog();
        }

        public void CountHystograms(int mode, out double[] R, out double[] G, out double[] B, out string rName,
            out string gName, out string bName)
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
                    switch (idx)
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
                        case 1:
                        {
                            double H, S, V;
                            lab1.ColorToHSV(color, out H, out S, out V);
                            rName = "Оттенок";
                            gName = "Насыщенность";
                            bName = "Яркость";
                            ++R[(int) (H / 360 * 255)];
                            ++G[(int) (S * 255)];
                            ++B[(int) (V * 255)];
                            break;
                        }
                        case 2:
                        {
                            double Y, U, V;
                            lab1.ColorToYUV(color, out Y, out U, out V);
                            rName = "Y";
                            gName = "U";
                            bName = "V";
                            ++R[(int) Y];
                            ++G[(int) U];
                            ++B[(int) V];
                            break;
                        }
                        case 3:
                        {
                            double H, S, L;
                            lab1.ColorToHSL(color, out H, out S, out L);
                            rName = "Оттенок";
                            gName = "Насыщенность";
                            bName = "Яркость";
                            ++R[(int) (H * 255)];
                            ++G[(int) (S * 255)];
                            ++B[(int) (L * 255)];
                            break;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] R, G, B;
            string rName, gName, bName;
            CountHystograms(idx, out R, out G, out B, out rName, out gName, out bName);

            HystogramForm hf = new HystogramForm(R, G, B, rName, gName, bName, this.Text);
            hf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Noize n = new Noize(orig, idx, pictureBox1);
            n.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sections s = new Sections(orig, idx, pictureBox1);
            s.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Equations eq = new Equations(idx, orig, pictureBox1);
            eq.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CorrecrionImage ci = new CorrecrionImage(orig, pictureBox1);
            ci.ShowDialog();
        }
    }
}
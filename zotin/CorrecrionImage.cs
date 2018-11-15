using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class CorrecrionImage : Form
    {
        private PictureBox pb;
        private Bitmap orig;
        public CorrecrionImage(Bitmap org, PictureBox pb)
        {
            InitializeComponent();

            this.pb = pb;
            this.orig = org;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(orig.Width, orig.Height);
            int Rmin = 255, Gmin = 255, Bmin = 255;
            int Rmax = 0, Gmax = 0, Bmax = 0;
            Color tempColor;

            //находим минимумы и максимумы R,G,B по изображению
            for (int x = 0; x < orig.Width; x++)
            {
                for (int y = 0; y < orig.Height; y++)
                {
                    tempColor = orig.GetPixel(x, y);
                    if (tempColor.R < Rmin) Rmin = tempColor.R;
                    if (tempColor.G < Gmin) Gmin = tempColor.G;
                    if (tempColor.B < Bmin) Bmin = tempColor.B;

                    if (tempColor.R > Rmax) Rmax = tempColor.R;
                    if (tempColor.G > Gmax) Gmax = tempColor.G;
                    if (tempColor.B > Bmax) Bmax = tempColor.B;
                }
            }
            //устанавливаем
            int R, G, B;
            for (int x = 0; x < orig.Width; x++)
            {
                for (int y = 0; y < orig.Height; y++)
                {
                    tempColor = orig.GetPixel(x, y);
                    R = (tempColor.R - Rmin) * (255 / (Rmax - Rmin));
                    G = (tempColor.G - Gmin) * (255 / (Gmax - Gmin));
                    B = (tempColor.B - Bmin) * (255 / (Bmax - Bmin));
                    temp.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            pb.Image = temp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(orig.Width, orig.Height);
            int Rmax = 0, Gmax = 0, Bmax = 0;
            Color tempColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //находим максимумы R,G,B по изображению
                for (int x = 0; x < orig.Width; x++)
                {
                    for (int y = 0; y < orig.Height; y++)
                    {
                        tempColor = orig.GetPixel(x, y);
                        if (tempColor.R > Rmax) Rmax = tempColor.R;
                        if (tempColor.G > Gmax) Gmax = tempColor.G;
                        if (tempColor.B > Bmax) Bmax = tempColor.B;
                    }
                }

                for (int i = 0; i < orig.Width; i++)
                {
                    for (int j = 0; j < orig.Height; j++)
                    {
                        temp.SetPixel(i, j,
                            Color.FromArgb(orig.GetPixel(i, j).R * colorDialog1.Color.R / Rmax,
                                orig.GetPixel(i, j).G * colorDialog1.Color.G / Gmax, orig.GetPixel(i, j).B * colorDialog1.Color.B / Bmax));
                    }
                }
                pb.Image = temp;
            }
        }
    }
}

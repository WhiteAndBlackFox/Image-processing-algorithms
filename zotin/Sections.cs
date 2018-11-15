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
    public partial class Sections : Form
    {
        private Bitmap org;
        private int mode;
        private PictureBox pb;
        public Sections(Bitmap origBitmap, int mode, PictureBox pb)
        {
            InitializeComponent();
            org = origBitmap;
            this.mode = mode;
            this.pb = pb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value >= numericUpDown2.Value)
            {
                MessageBox.Show("Нижняя граница не может быть больше вверхней!!");
                return;
            }
            Bitmap result = new Bitmap(org);
            Color color;
            //повторить для каждого пикселя на изображении в процентном отношении (т.е. noizeLevel процентов)
            for (int i = 0; i < org.Width; i++)
            {
                for (int j = 0; j < org.Height; j++)
                {
                    color = org.GetPixel(i, j);

                    switch (mode)
                    {
                        #region RGB processing
                        case 0:
                        {
                            int[] colors = new int[3] {color.R, color.G, color.B};
                            for (int idx = 0; idx < 3; idx++)
                            {
                                if (colors[idx] > numericUpDown1.Value && colors[idx] < numericUpDown2.Value)
                                    colors[idx] = 0;
                            }
                            result.SetPixel(i,j,Color.FromArgb(colors[0], colors[1], colors[2]));
                            pb.Image = result;
                            break;
                        }
                        #endregion

                        #region HSV processing
                        case 2:
                        {
                            
                            double[] colorsHSV = new double[3];
                            lab1.ColorToHSV(color, out colorsHSV[0], out colorsHSV[1], out colorsHSV[2]);

                            if (colorsHSV[0] > (double)((numericUpDown1.Value + 10) / 100) && colorsHSV[0] < (double)((numericUpDown2.Value + 10) / 100))
                                colorsHSV[0] = 0;

                            if (colorsHSV[1] > (double)((numericUpDown1.Value + 10) / 100) && colorsHSV[1] < (double)((numericUpDown2.Value + 10) / 100))
                                colorsHSV[1] = 0;

                            if (colorsHSV[2] > (double)((numericUpDown1.Value + 10) / 100) && colorsHSV[2] < (double)((numericUpDown2.Value + 10) / 100))
                                colorsHSV[2] = 0;

                            lab1.HSVToColor(colorsHSV[0], colorsHSV[1], colorsHSV[2]);
                            result.SetPixel(i, j, lab1.HSVToColor(colorsHSV[0], colorsHSV[1], colorsHSV[2]));
                            pb.Image = result;
                            break;
                        }
                        #endregion

                        #region YUV processing
                        case 1:
                        {
                            double[] colorsYUV = new double[3];
                            lab1.ColorToYUV(color, out colorsYUV[0], out colorsYUV[1], out colorsYUV[2]);
                            
                            if (colorsYUV[0] > (double) numericUpDown1.Value && colorsYUV[0] < (double) numericUpDown2.Value)
                                colorsYUV[0] = 0;

                            if (colorsYUV[1] > (double)numericUpDown1.Value && colorsYUV[1] < (double)numericUpDown2.Value)
                                colorsYUV[1] = 0;

                            if (colorsYUV[2] > (double)numericUpDown1.Value && colorsYUV[2] < (double)numericUpDown2.Value)
                                colorsYUV[2] = 0;

                            result.SetPixel(i, j, Color.FromArgb((int)colorsYUV[0], (int)colorsYUV[1], (int)colorsYUV[2]));
                            pb.Image = result;
                            break;
                        }
                        #endregion

                        #region HSL processing
                        case 3:
                        {
                            double[] colorsHSL = new double[3];
                            lab1.ColorToHSL(color, out colorsHSL[0], out colorsHSL[1], out colorsHSL[2]);

                            if (colorsHSL[0] > (double)numericUpDown1.Value / 100 && colorsHSL[0] < (double)numericUpDown2.Value / 100)
                                colorsHSL[0] = 0;

                            if (colorsHSL[1] > (double)numericUpDown1.Value / 100 && colorsHSL[1] < (double)numericUpDown2.Value / 100)
                                colorsHSL[1] = 0;

                            if (colorsHSL[2] > (double)numericUpDown1.Value / 100 && colorsHSL[2] < (double)numericUpDown2.Value / 100)
                                colorsHSL[2] = 0;

                            result.SetPixel(i, j, lab1.HSLToColor(colorsHSL[0], colorsHSL[1], colorsHSL[2]));
                            pb.Image = result;
                            break;
                        }
                        #endregion
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(org.Width, org.Height);
            for (int i = 0; i < org.Width; i++)
            {
                for (int j = 0; j < org.Height; j++)
                {
                    Color c = org.GetPixel(i, j);

                    res.SetPixel(i, j, Color.FromArgb((int)(numericUpDown1.Value * (decimal)Math.Log10(1 + c.R)), (int)(numericUpDown1.Value * (decimal)Math.Log10(1 + c.G)), (int)(numericUpDown1.Value * (decimal)Math.Log10(1 + c.B))));
                }
            }
            pb.Image = res;
        }
    }
}

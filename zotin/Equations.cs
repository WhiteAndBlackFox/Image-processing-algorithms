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
    public partial class Equations : Form
    {
        Bitmap bmp1, orig;
        private PictureBox pb;
        private int idx;
        public Equations(int mode, Bitmap orig, PictureBox pb)
        {
            InitializeComponent();
            this.pb = pb;
            this.orig = orig;
            idx = mode;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pb.Image = orig;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(orig.Width, orig.Height);
            for (int i = 0; i < orig.Width; i++)
            {
                for (int j = 0; j < orig.Height; j++)
                {
                    Color c = orig.GetPixel(i, j);

                    res.SetPixel(i, j, Color.FromArgb((int) (numericUpDown1.Value * (decimal) Math.Log10(1 + c.R)), (int) (numericUpDown1.Value * (decimal) Math.Log10(1 + c.G)), (int) (numericUpDown1.Value * (decimal) Math.Log10(1 + c.B))));
                }
            }
            pb.Image = res;
        }
    }
}

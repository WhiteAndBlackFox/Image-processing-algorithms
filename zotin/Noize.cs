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
    public partial class Noize : Form
    {
        Bitmap image;
        private int idx;
        private PictureBox pb;
        public Noize(Bitmap original, int idx, PictureBox pb)
        {
            InitializeComponent();
            image = original;
            this.idx = idx;
            this.pb = pb;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Уровень \nшума " + trackBar1.Value + "%";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label5.Text = "Уровень \nшума " + trackBar2.Value + "%";
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label6.Text = "Уровень \nшума " + trackBar3.Value + "%";
        }
        private int determineChannel()
        {
            if (radioButton1.Checked)
                return 0;
            else if (radioButton2.Checked)
                return 1;
            else if (radioButton3.Checked)
                return 2;
            else if (radioButton4.Checked)
                return 3;
            else
                return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NoizeGenerator ng = new NoizeGenerator();
            pb.Image = ng.applyImpulseNoize(idx, image, trackBar1.Value, (int)numericUpDown1.Value, determineChannel());
        }

        private void button4_Click(object sender, EventArgs e)
        {

            NoizeGenerator ng = new NoizeGenerator();
            pb.Image = ng.applyAdditiveNoize(idx, image, trackBar2.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, determineChannel());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NoizeGenerator ng = new NoizeGenerator();
            pb.Image = ng.applyMultiplicativeNoize(idx, image, trackBar3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value, determineChannel());
        }


    }
}

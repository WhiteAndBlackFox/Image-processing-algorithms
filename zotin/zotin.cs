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
    public partial class zotin : Form
    {
        public zotin()
        {
            InitializeComponent();
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void лаб1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                lab11 l1 = new lab11(pictureBox1);
                l1.imgOrig = new Bitmap(pictureBox1.Image);
                l1.Show();
            }
            else
                MessageBox.Show("Не могу работать с пустотой!!! \nВыбери изображение!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void выбратьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "img files (*.jpg)|*.jpg|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName != null) {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    pictureBox2.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void лаб2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lab2 l2 = new lab2(new Bitmap(pictureBox1.Image), pictureBox1, new Bitmap(pictureBox2.Image));
            l2.Show();
        }

        private void лаб3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lab3 l3 = new lab3(new Bitmap(pictureBox1.Image), pictureBox1);
            l3.Show();
        }

        private void лаб4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lab4 l4 = new lab4(new Bitmap(pictureBox1.Image), pictureBox1);
            l4.Show();
        }
    }
}

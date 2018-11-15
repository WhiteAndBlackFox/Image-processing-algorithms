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
    public partial class ColorBalance : Form
    {
        private Bitmap orig;
        private PictureBox pb;
        public ColorBalance(PictureBox form, Bitmap org, int idx)
        {
            InitializeComponent();

            pb = form;
            orig = org;

            switch (idx)
            {
                case 0:
                {
                    label2.Text = "R";
                    label4.Text = "G";
                    label6.Text = "B";
                    break;
                }
                case 1:
                {
                    label2.Text = "H";
                    label4.Text = "S";
                    label6.Text = "V";
                    break;
                }
                case 2:
                {
                    label2.Text = "Y";
                    label4.Text = "U";
                    label6.Text = "V";
                    break;
                }
                case 3:
                {
                    label2.Text = "H";
                    label4.Text = "S";
                    label6.Text = "L";
                    break;
                }
            }
        }

        //цветовой баланс R

        private void setR(Bitmap origBitmap)
        {
            Bitmap res = new Bitmap(origBitmap);
            UInt32 p;
            for (int i = 0; i < origBitmap.Width; i++)
                for (int j = 0; j < origBitmap.Height; j++)
                {
                    p = ColorBalance_R((UInt32)origBitmap.GetPixel(i, j).ToArgb(), trackBar1.Value, trackBar1.Maximum);
                    res.SetPixel(i, j, Color.FromArgb((int)p));
                }
            pb.Image = res;
        }

        //цветовой баланс G
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Green " + (100 + trackBar2.Value * 10).ToString() + "%";
        }

        private void setG(Bitmap origBitmap)
        {
            Bitmap res = new Bitmap(origBitmap);
            UInt32 p;
            for (int i = 0; i < origBitmap.Width; i++)
                for (int j = 0; j < origBitmap.Height; j++)
                {
                    p = ColorBalance_G((UInt32)origBitmap.GetPixel(i, j).ToArgb(), trackBar2.Value, trackBar2.Maximum);
                    res.SetPixel(i, j, Color.FromArgb((int)p));
                }
            pb.Image = res;
        }

        //цветовой баланс B
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            //setB();
            label6.Text = "Blue " + (100 + trackBar3.Value * 10).ToString() + "%";
        }

        private void setB(Bitmap origBitmap)
        {
            Bitmap res = new Bitmap(origBitmap);
            UInt32 p;
            for (int i = 0; i < origBitmap.Width; i++)
                for (int j = 0; j < origBitmap.Height; j++)
                {
                    p = ColorBalance_B((UInt32)origBitmap.GetPixel(i, j).ToArgb(), trackBar3.Value, trackBar3.Maximum);
                    res.SetPixel(i, j, Color.FromArgb((int)p));
                }
            pb.Image = res;
        }

        private void ColorBalanceForm_Load(object sender, EventArgs e)
        {

        }


        //цветовой баланс R
        public UInt32 ColorBalance_R(UInt32 point, int poz, int lenght)
        {
            int R;
            int G;
            int B;

            int N = (100 / lenght) * poz; //кол-во процентов

            R = (int)(((point & 0x00FF0000) >> 16) + N * 128 / 100);
            G = (int)((point & 0x0000FF00) >> 8);
            B = (int)(point & 0x000000FF);

            //контролируем переполнение переменных
            if (R < 0) R = 0;
            if (R > 255) R = 255;

            point = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);

            return point;
        }

        //цветовой баланс G
        public UInt32 ColorBalance_G(UInt32 point, int poz, int lenght)
        {
            int R;
            int G;
            int B;

            int N = (100 / lenght) * poz; //кол-во процентов

            R = (int)((point & 0x00FF0000) >> 16);
            G = (int)(((point & 0x0000FF00) >> 8) + N * 128 / 100);
            B = (int)(point & 0x000000FF);

            //контролируем переполнение переменных
            if (G < 0) G = 0;
            if (G > 255) G = 255;

            point = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);

            return point;
        }

        //цветовой баланс B
        public UInt32 ColorBalance_B(UInt32 point, int poz, int lenght)
        {
            int R;
            int G;
            int B;

            int N = (100 / lenght) * poz; //кол-во процентов

            R = (int)((point & 0x00FF0000) >> 16);
            G = (int)((point & 0x0000FF00) >> 8);
            B = (int)((point & 0x000000FF) + N * 128 / 100);

            //контролируем переполнение переменных
            if (B < 0) B = 0;
            if (B > 255) B = 255;

            point = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);

            return point;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setR(orig);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            setG(orig);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setB(orig);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(orig);
            for (int i = 0; i < orig.Width; i++)
                for (int j = 0; j < orig.Height; j++)
                {
                    Color color = orig.GetPixel(i, j);
                    double h, s, v;
                    lab1.ColorToHSV(color, out h, out s, out v);

                    h = h + trackBar4.Value;

                    // корректировки
                    if (h > 360)
                        h = h - 360;
                    if (h < 0)
                        h = h + 360;

                    color = lab1.HSVToColor(h, s, v);
                    res.SetPixel(i, j, color);
                }
            pb.Image = res;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(orig);
            for (int i = 0; i < orig.Width; i++)
                for (int j = 0; j < orig.Height; j++)
                {
                    Color color = orig.GetPixel(i, j);
                    double h, s, v;
                    lab1.ColorToHSV(color, out h, out s, out v);

                    s = s + (s / 10 * trackBar5.Value);

                    if (s > 1)
                        s = 1;
                    else if (s < 0)
                        s = 0;

                    color = lab1.HSVToColor(h, s, v);
                    res.SetPixel(i, j, color);
                }
            pb.Image = res;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(orig);
            for (int i = 0; i < orig.Width; i++)
                for (int j = 0; j < orig.Height; j++)
                {
                    Color color = orig.GetPixel(i, j);
                    double h, s, l;
                    lab1.ColorToHSL(color, out h, out s, out l);

                    l = l + (l / 10 * trackBar6.Value);

                    if (l > 1)
                        l = 1;
                    else if (l < 0)
                        l = 0;

                    color = lab1.HSLToColor(h, s, l);
                    res.SetPixel(i, j, color);
                }
            pb.Image = res;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap res = new Bitmap(orig);
            double contrastValue = Math.Pow((100.0 + (trackBar8.Value * 10)) / 100.0, 2);
            for (int i = 0; i < orig.Width; i++)
                for (int j = 0; j < orig.Height; j++)
                {
                    int r, g, b;
                    Color color = orig.GetPixel(i, j);
                    r = (int)(((((color.R / 255.0) - 0.5) * contrastValue) + 0.5) * 255);
                    g = (int)(((((color.G / 255.0) - 0.5) * contrastValue) + 0.5) * 255);
                    b = (int)(((((color.B / 255.0) - 0.5) * contrastValue) + 0.5) * 255);

                    if (r > 255)
                        r = 255;
                    else if (r < 0)
                        r = 0;

                    if (g > 255)
                        g = 255;
                    else if (g < 0)
                        g = 0;

                    if (b > 255)
                        b = 255;
                    else if (b < 0)
                        b = 0;
                    res.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            pb.Image = res;
        }
    }
}

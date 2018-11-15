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
    public partial class lab1 : Form
    {
        public Bitmap imgOrig;
        Bitmap res1, res2, res3, res4;
        private ColorModels cm1, cm2, cm3, cm4;
        private double h, s, l, v, Y, u;

        public lab1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public static Color HSLToColor(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;
                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            Color rgb = Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
            return rgb;
        }

        public static void ColorToHSL(Color rgb, out double h, out double s, out double l)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;
            double v;
            double m;
            double vm;
            double r2, g2, b2;

            h = 0; 
            s = 0;
            l = 0;

            v = Math.Max(r, g);
            v = Math.Max(v, b);
            m = Math.Min(r, g);
            m = Math.Min(m, b);

            l = (m + v) / 2.0;
            if (l <= 0.0)
            {
                return;
            }

            vm = v - m;
            s = vm;

            if (s > 0.0)
            {
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            }
            else
            {
                return;
            }

            r2 = (v - r) / vm;
            g2 = (v - g) / vm;
            b2 = (v - b) / vm;

            if (r == v)
            {
                h = (g == m ? 5.0 + b2 : 1.0 - g2);
            }
            else if (g == v)
            {
                h = (b == m ? 1.0 + r2 : 3.0 - b2);
            }
            else
            {
                h = (r == m ? 3.0 + g2 : 5.0 - r2);
            }
            h /= 6.0;
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1 - (1 * (double)min / (double)max);
            value = (double)max / 255;
        }

        public static Color HSVToColor(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private void toHSV_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < imgOrig.Width; x++)
            {
                for (int y = 0; y < imgOrig.Height; y++)
                {
                    ColorToHSV(imgOrig.GetPixel(x, y), out h, out s, out v);

                    res1.SetPixel(x, y, HSVToColor(h, 1, 0.5));
                    res2.SetPixel(x, y, HSVToColor(h, s, 0.5));
                    res3.SetPixel(x, y, HSVToColor(h, 1, v));
                }
            }
            
            cm1 = new ColorModels("H", res1, 1);
            cm1.Show();
            cm2 = new ColorModels("S", res2, 1);
            cm2.Show();
            cm3 = new ColorModels("V", res3, 1);
            cm3.Show();
        }

        private void toHSL_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < imgOrig.Width; x++)
            {
                for (int y = 0; y < imgOrig.Height; y++)
                {
                    ColorToHSL(imgOrig.GetPixel(x, y), out h, out s, out l);

                    res1.SetPixel(x, y, HSLToColor(h, 1, 0.5));
                    res2.SetPixel(x, y, HSLToColor(h, s, 0.5));
                    res3.SetPixel(x, y, HSLToColor(h, 1, l));
                }
            }

            cm1 = new ColorModels("H", res1, 3);
            cm1.Show();
            cm2 = new ColorModels("S", res2, 3);
            cm2.Show();
            cm3 = new ColorModels("L", res3, 3);
            cm3.Show();
        }



        private void lab1_Shown(object sender, EventArgs e)
        {
            res1 = new Bitmap(imgOrig.Width, imgOrig.Height);
            res2 = new Bitmap(imgOrig.Width, imgOrig.Height);
            res3 = new Bitmap(imgOrig.Width, imgOrig.Height);
            res4 = new Bitmap(imgOrig.Width, imgOrig.Height);
        }

        public static void ColorToYUV(Color color, out double Y, out double U, out double V)
        {
            Y = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            U = -0.147 * color.R - 0.289 * color.G + 0.436 * color.B + 128;
            V = 0.615 * color.R - 0.515 * color.G - 0.100 * color.B + 128;
        }

        public Color YUVToColor(double Y, double U, double V)
        {
            double r, g, b;

            r = Y + 1.14 * (V - 128);
            g = Y - 0.395 * (U - 128) - 0.581 * (V - 128);
            b = Y + 2.032 * (U - 128);

            return Color.FromArgb(TrimRGB(r), TrimRGB(g), TrimRGB(b));
        }
        public static int TrimRGB(double value)
        {
            if (value > 255)
                return 255;
            else if (value < 0)
                return 0;
            else
                return (int)value;
        }

        private void toYUV_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < imgOrig.Width; x++)
            {
                for (int y = 0; y < imgOrig.Height; y++)
                {
                    ColorToYUV(imgOrig.GetPixel(x, y), out Y, out u, out v);

                    res1.SetPixel(x, y, YUVToColor(Y, 128, 128));
                    res2.SetPixel(x, y, YUVToColor(128, u, 128));
                    res3.SetPixel(x, y, YUVToColor(128, 128, v));
                    res4.SetPixel(x, y, Color.FromArgb(TrimRGB(Y), TrimRGB(u), TrimRGB(v)));
                }
            }

            cm1 = new ColorModels("Y", res1, 2);
            cm1.Show();
            cm2 = new ColorModels("U", res2, 2);
            cm2.Show();
            cm3 = new ColorModels("V", res3, 2);
            cm3.Show();
            cm4 = new ColorModels("YUV", res4, 2);
            cm4.Show();
        }

        private void toRGB_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < imgOrig.Width; x++)
            {
                for (int y = 0; y < imgOrig.Height; y++)
                {
                    res1.SetPixel(x, y, Color.FromArgb(imgOrig.GetPixel(x, y).R, 0, 0));
                    res2.SetPixel(x, y, Color.FromArgb(0, imgOrig.GetPixel(x, y).G, 0));
                    res3.SetPixel(x, y, Color.FromArgb(0, 0, imgOrig.GetPixel(x, y).B));

                    int grayscale = (int)((imgOrig.GetPixel(x, y).R * 0.299) + (imgOrig.GetPixel(x, y).G * 0.587) + (imgOrig.GetPixel(x, y).B * 0.114));
                    res4.SetPixel(x, y, Color.FromArgb(grayscale, grayscale, grayscale));
                }
            }
            cm1 = new ColorModels("R", res1, 0);
            cm1.Show();
            cm2 = new ColorModels("G", res2, 0);
            cm2.Show();
            cm3 = new ColorModels("B", res3, 0);
            cm3.Show();
            cm4 = new ColorModels("GrayScale", res4, 0);
            cm4.Show();
        }
    }
}

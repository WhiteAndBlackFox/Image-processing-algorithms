using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    class NoizeGenerator
    {
        Random rand = new Random();

        public Bitmap applyNoise(Bitmap originalImage, string mode, int colorMode, int noizeLevel, double k, int additionalK = 0, int secondAdditionalK = 0, int colorChannel = 0)
        {
            Bitmap result = new Bitmap(originalImage);

            switch (mode.Trim().ToUpper())
            {
                case "IMPULSE":
                    result = applyImpulseNoize(colorMode, originalImage, noizeLevel, k, colorChannel);
                    break;
                case "ADDITIVE":
                    result = applyAdditiveNoize(colorMode, originalImage, noizeLevel, additionalK, secondAdditionalK, colorChannel);
                    break;
                case "MULTIPLICATIVE":
                    result = applyMultiplicativeNoize(colorMode, originalImage, noizeLevel, additionalK, secondAdditionalK, colorChannel);
                    break;
                default:
                    MessageBox.Show("Ошибка распознавания режима генерации шума");
                    break;
            }
            return result;
        }

        public Bitmap applyImpulseNoize(int colorMode, Bitmap originalImage, int noizeLevel, double impulseK, int colorChannel = 0)
        {
            Bitmap result = new Bitmap(originalImage);
            int noisepixel = (int)(originalImage.Width * originalImage.Height * noizeLevel / 100);
            int x, y, randomValue;
            Color color, tempColor;

            //повторить для каждого пикселя на изображении в процентном отношении (т.е. noizeLevel процентов)
            for (int i = 0; i < noisepixel; i++)
            {
                //получаем случайный пиксель на изображении и случайное значение
                x = rand.Next(0, originalImage.Width);
                y = rand.Next(0, originalImage.Height);
                color = originalImage.GetPixel(x, y);
                randomValue = rand.Next(0, 100);

                //обработка RGB и YUV
                #region RGB and YUV processing
                if (colorMode == 0 || colorMode == 1)
                {
                    //массив с цветами, нужен для RGB и YUV
                    int[] colors = new int[3];
                    colors[0] = color.R;
                    colors[1] = color.G;
                    colors[2] = color.B;

                    //если канал - базовый (например полный RGB)
                    if (colorChannel == 0)
                    {
                        if (randomValue < impulseK)
                            result.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        else
                            result.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        if (randomValue < impulseK)
                        {
                            //зануляем соответстствующий канал
                            colors[colorChannel - 1] = 0;
                            result.SetPixel(x, y, Color.FromArgb(colors[0], colors[1], colors[2]));
                        }
                        else
                        {
                            colors[colorChannel - 1] = 255;
                            result.SetPixel(x, y, Color.FromArgb(colors[0], colors[1], colors[2]));
                        }
                    }
                }
                #endregion rgb

                #region HSL and HSV
                else if (colorMode == 3)
                {
                    double H, S, L;
                    lab1.ColorToHSL(color, out H, out S, out L);

                    //All
                    if (colorChannel == 0)
                    {
                        if (randomValue < impulseK)
                        {
                            H -= 180;
                            S = L = 0;
                        }
                        else
                        {
                            H += 180;
                            S = L = 1;
                        }
                    }
                    //Hue
                    else if (colorChannel == 1)
                    {
                        if (randomValue < impulseK)
                            H -= 180;
                        else
                            H += 180;
                    }
                    //S
                    else if (colorChannel == 2)
                    {
                        if (randomValue < impulseK)
                            S = 0;
                        else
                            S = 1;
                    }
                    //L
                    else if (colorChannel == 3)
                    {
                        if (randomValue < impulseK)
                            L = 0;
                        else
                            L = 1;
                    }

                    H = CorrectHue(H);
                    tempColor = lab1.HSLToColor(H, S, L);
                    result.SetPixel(x, y, tempColor);
                }
                else if (colorMode == 2)
                {
                    double H, S, V;
                    lab1.ColorToHSV(color, out H, out S, out V);

                    //All
                    if (colorChannel == 0)
                    {
                        if (randomValue < impulseK)
                        {
                            H -= 180;
                            S = V = 0;
                        }
                        else
                        {
                            H += 180;
                            S = V = 1;
                        }
                    }
                    //Hue
                    else if (colorChannel == 1)
                    {
                        if (randomValue < impulseK)
                            H -= 180;
                        else
                            H += 180;
                    }
                    //S
                    else if (colorChannel == 2)
                    {
                        if (randomValue < impulseK)
                            S = 0;
                        else
                            S = 1;
                    }
                    //V
                    else if (colorChannel == 3)
                    {
                        if (randomValue < impulseK)
                            V = 0;
                        else
                            V = 1;
                    }

                    H = CorrectHue(H);
                    tempColor = lab1.HSVToColor(H, S, V);
                    result.SetPixel(x, y, tempColor);
                }
                #endregion
                else
                {
                    MessageBox.Show("Неизвестный цветовой режим");
                }
            }

            return result;
        }

        public double CorrectHue(double H)
        {
            if (H > 360)
                H = H - 360;
            else if (H < 360)
                H = H + 360;

            return H;
        }

        public int getAddiviteAdditionalValue(int colorNumber, int min, int max, string mode)
        {
            int color = colorNumber + rand.Next(min, max);

            if (mode == "RGB")
            {
                if (color > 255)
                    color = 255;
                else if (color < 0)
                    color = 0;
            }
            else if (mode == "H")
            {
                if (color > 360)
                    color -= 360;
                else if (color < 0)
                    color += 360;
            }
            else if (mode == "S" || mode == "L" || mode == "V")
            {
                if (color > 1)
                    color = 1;
                else if (color < 0)
                    color = 0;
            }

            return color;
        }

        public int getMultiplicativeAdditionalValue(int colorNumber, int min, int max, string mode)
        {
            int color = colorNumber * rand.Next(min, max);

            if (mode == "RGB")
            {
                if (Math.Abs(color) > 2 * 255)
                    color = 0;

                if (color > 255)
                    color = 255;
                else if (color < 0)
                    color = 0;
            }
            else if (mode == "H")
            {
                if (Math.Abs(color) > 2 * 360)
                    color = 0;

                if (color > 360)
                    color -= 360;
                else if (color < 0)
                    color += 360;
            }
            else if (mode == "S" || mode == "L" || mode == "V")
            {
                if (Math.Abs(color) > 2 * 1)
                    color = 0;

                if (color > 1)
                    color = 1;
                else if (color < 0)
                    color = 0;
            }

            return color;
        }

        public Bitmap applyAdditiveNoize(int colorMode, Bitmap originalImage, int noizeLevel, int min, int max, int colorChannel = 0)
        {
            Bitmap result = new Bitmap(originalImage);
            int noisepixel = (int)(originalImage.Width * originalImage.Height * noizeLevel / 100);
            int x, y, randomValue;
            Color color;
            double value1, value2, value3;

            //повторить для каждого пикселя на изображении в процентном отношении (т.е. noizeLevel процентов)
            for (int i = 0; i < noisepixel; i++)
            {
                //получаем случайный пиксель на изображении и случайное значение
                x = rand.Next(0, originalImage.Width);
                y = rand.Next(0, originalImage.Height);
                color = originalImage.GetPixel(x, y);
                randomValue = rand.Next(0, 100);

                if (colorMode == 0 || colorMode == 1)
                {
                    if (colorChannel == 0)
                    {
                        value1 = getAddiviteAdditionalValue(color.R, min, max, "RGB");
                        value2 = getAddiviteAdditionalValue(color.G, min, max, "RGB");
                        value3 = getAddiviteAdditionalValue(color.B, min, max, "RGB");
                        result.SetPixel(x, y, Color.FromArgb((int)value1, (int)value2, (int)value3));
                    }
                    else
                    {
                        //массив с цветами, нужен для RGB и YUV
                        int[] colors = new int[3];
                        colors[0] = color.R;
                        colors[1] = color.G;
                        colors[2] = color.B;

                        colors[colorChannel - 1] = getAddiviteAdditionalValue(colors[colorChannel - 1], min, max, "RGB");

                        result.SetPixel(x, y, Color.FromArgb(colors[0], colors[1], colors[2]));
                    }
                }
                else if (colorMode == 3)
                {
                    double H, S, L;
                    lab1.ColorToHSL(color, out H, out S, out L);
                    if (colorChannel == 0)
                    {
                        value1 = getAddiviteAdditionalValue((int)H, min, max, "H");
                        value2 = getAddiviteAdditionalValue((int)(S * 100), min, max, "S");
                        value3 = getAddiviteAdditionalValue((int)(L * 100), min, max, "L");
                        result.SetPixel(x, y, lab1.HSLToColor(value1, value2, value3));
                    }
                    else if (colorChannel == 1)
                    {
                        value1 = getAddiviteAdditionalValue((int)H, min, max, "H");
                        result.SetPixel(x, y, lab1.HSLToColor(value1, S, L));
                    }
                    else if (colorChannel == 2)
                    {
                        value2 = getAddiviteAdditionalValue((int)(S * 100), min, max, "S");
                        result.SetPixel(x, y, lab1.HSLToColor(H, value2, L));
                    }
                    else if (colorChannel == 3)
                    {
                        value3 = getAddiviteAdditionalValue((int)(L * 100), min, max, "L");
                        result.SetPixel(x, y, lab1.HSLToColor(H, S, value3));
                    }
                }
                else if (colorMode == 2)
                {
                    double H, S, V;
                    lab1.ColorToHSV(color, out H, out S, out V);
                    if (colorChannel == 0)
                    {
                        value1 = getAddiviteAdditionalValue((int)H, min, max, "H");
                        value2 = getAddiviteAdditionalValue((int)(S * 100), min, max, "S");
                        value3 = getAddiviteAdditionalValue((int)(V * 100), min, max, "V");
                        result.SetPixel(x, y, lab1.HSVToColor(value1, value2, value3));
                    }
                    else if (colorChannel == 1)
                    {
                        value1 = getAddiviteAdditionalValue((int)H, min, max, "H");
                        result.SetPixel(x, y, lab1.HSVToColor(value1, S, V));
                    }
                    else if (colorChannel == 2)
                    {
                        value2 = getAddiviteAdditionalValue((int)(S * 100), min, max, "S");
                        result.SetPixel(x, y, lab1.HSVToColor(H, value2, V));
                    }
                    else if (colorChannel == 3)
                    {
                        value3 = getAddiviteAdditionalValue((int)(V * 100), min, max, "V");
                        result.SetPixel(x, y, lab1.HSVToColor(H, S, value3));
                    }
                }
                else
                {
                    MessageBox.Show("Неизвестный цветовой режим");
                }
            }
            return result;
        }

        public Bitmap applyMultiplicativeNoize(int colorMode, Bitmap originalImage, int noizeLevel, int kmin, int kmax, int colorChannel = 0)
        {
            Bitmap result = new Bitmap(originalImage);
            int noisepixel = (int)(originalImage.Width * originalImage.Height * noizeLevel / 100);
            int x, y, randomValue;
            Color color;
            double value1, value2, value3;

            //повторить для каждого пикселя на изображении в процентном отношении (т.е. noizeLevel процентов)
            for (int i = 0; i < noisepixel; i++)
            {
                //получаем случайный пиксель на изображении и случайное значение
                x = rand.Next(0, originalImage.Width);
                y = rand.Next(0, originalImage.Height);
                color = originalImage.GetPixel(x, y);
                randomValue = rand.Next(0, 100);

                if (colorMode == 0 || colorMode == 2)
                {
                    if (colorChannel == 0)
                    {
                        value1 = getMultiplicativeAdditionalValue(color.R, kmin, kmax, "RGB");
                        value2 = getMultiplicativeAdditionalValue(color.G, kmin, kmax, "RGB");
                        value3 = getMultiplicativeAdditionalValue(color.B, kmin, kmax, "RGB");
                        result.SetPixel(x, y, Color.FromArgb((int)value1, (int)value2, (int)value3));
                    }
                    else
                    {
                        //массив с цветами, нужен для RGB и YUV
                        int[] colors = new int[3];
                        colors[0] = color.R;
                        colors[1] = color.G;
                        colors[2] = color.B;

                        colors[colorChannel - 1] = getMultiplicativeAdditionalValue(colors[colorChannel - 1], kmin, kmax, "RGB");

                        result.SetPixel(x, y, Color.FromArgb(colors[0], colors[1], colors[2]));
                    }
                }
                else if (colorMode == 3)
                {
                    double H, S, L;
                    lab1.ColorToHSL(color, out H, out S, out L);
                    if (colorChannel == 0)
                    {
                        value1 = getMultiplicativeAdditionalValue((int)H, kmin, kmax, "H");
                        value2 = getMultiplicativeAdditionalValue((int)(S * 100), kmin, kmax, "S");
                        value3 = getMultiplicativeAdditionalValue((int)(L * 100), kmin, kmax, "L");
                        result.SetPixel(x, y, lab1.HSLToColor(value1, value2, value3));
                    }
                    else if (colorChannel == 1)
                    {
                        value1 = getMultiplicativeAdditionalValue((int)H, kmin, kmax, "H");
                        result.SetPixel(x, y, lab1.HSLToColor(value1, S, L));
                    }
                    else if (colorChannel == 2)
                    {
                        value2 = getMultiplicativeAdditionalValue((int)(S * 100), kmin, kmax, "S");
                        result.SetPixel(x, y, lab1.HSLToColor(H, value2, L));
                    }
                    else if (colorChannel == 3)
                    {
                        value3 = getMultiplicativeAdditionalValue((int)(L * 100), kmin, kmax, "L");
                        result.SetPixel(x, y, lab1.HSLToColor(H, S, value3));
                    }
                }
                else if (colorMode == 1)
                {
                    double H, S, V;
                    lab1.ColorToHSV(color, out H, out S, out V);
                    if (colorChannel == 0)
                    {
                        value1 = getMultiplicativeAdditionalValue((int)H, kmin, kmax, "H");
                        value2 = getMultiplicativeAdditionalValue((int)(S * 100), kmin, kmax, "S");
                        value3 = getMultiplicativeAdditionalValue((int)(V * 100), kmin, kmax, "V");
                        result.SetPixel(x, y, lab1.HSVToColor(value1, value2, value3));
                    }
                    else if (colorChannel == 1)
                    {
                        value1 = getMultiplicativeAdditionalValue((int)H, kmin, kmax, "H");
                        result.SetPixel(x, y, lab1.HSVToColor(value1, S, V));
                    }
                    else if (colorChannel == 2)
                    {
                        value2 = getMultiplicativeAdditionalValue((int)(S * 100), kmin, kmax, "S");
                        result.SetPixel(x, y, lab1.HSVToColor(H, value2, V));
                    }
                    else if (colorChannel == 3)
                    {
                        value3 = getMultiplicativeAdditionalValue((int)(V * 100), kmin, kmax, "V");
                        result.SetPixel(x, y, lab1.HSVToColor(H, S, value3));
                    }
                }
                else
                {
                    MessageBox.Show("Неизвестный цветовой режим");
                }
            }

            return result;
        }
    }
}

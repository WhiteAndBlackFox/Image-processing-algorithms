using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace zotin
{
    public class ImageProcessor
    {

        #region Constructor & private fields
        private const byte Black = 0;
        private const byte White = 255;

        private static Image _image;
        private byte[] _original;
        private static byte[] _primaryImage;
        private static Action<WorkItem, MetricsInfo> _imageProcessResult;
        private static double _time;
        private static string _function;
        private int _pixelCount;
        private static int _runTestCount;

        protected static int Width;
        protected static int Height;
        protected static int Stride;
        protected readonly Random Random;

        public ImageProcessor()
        {
            _image = null;
            Random = new Random(new DateTime().Millisecond);
            _runTestCount = 1;
        }

        #endregion

        #region Public class methods

        public Image Image { get { return _image; } }

        public static WorkItem LastWorkItem
        {
            get
            {
                return new WorkItem
                {
                    Function = _function,
                    ImgWidth = Width,
                    ImgHeight = Height,
                    Time = _time
                };
            }
        }

        public void Open(Image image, bool primary)
        {
            _image = image;
            Width = image.Width;
            Height = image.Height;
            _pixelCount = Width * Height;
            _function = null;
            //Byte buffer for original image
            ReadBytes(ImageLockMode.ReadOnly, (buffer, bitmapData) =>
            {
                _original = buffer;
                if (primary)
                {
                    _primaryImage = buffer.ToArray();
                }
            });
        }

        public void SetRunTestCount(int runTestCount)
        {
            _runTestCount = runTestCount;
        }

        public bool IsImageOpened
        {
            get { return _image != null; }
        }

        #endregion

        #region GammaCorrection

        public Bitmap GammaCorrection(double value)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                var rampTable = new byte[256];
                for (int i = 0; i < rampTable.Length; i++)
                {
                    var temp = value * Math.Log(1 + i / 255) * 255;
                    rampTable[i] = (byte)Math.Min(255, temp);
                }

                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        buffer[pixelIndex + 2] = rampTable[buffer[pixelIndex + 2]];
                        buffer[pixelIndex + 1] = rampTable[buffer[pixelIndex + 1]];
                        buffer[pixelIndex] = rampTable[buffer[pixelIndex]];
                    }
                }
            });
        }
        #endregion

        #region PartialLinearCorrection
        public Bitmap PartialLinearCorrection(Point[] points)
        {
            _function = string.Format("PartialLinearCorrection (points: {0})", points.Length);
            IDictionary<byte, byte> dictionary = new Dictionary<byte, byte>();
            for (byte i = 0; i < points.Length - 1; i++)
            {
                Point a = points[i];
                Point b = points[i + 1];
                double p1 = (double)(b.Y - a.Y) / (b.X - a.X);
                double p2 = (double)(b.X * a.Y - a.X * b.Y) / (b.X - a.X);
                for (int x = a.X; x < b.X; x++)
                {
                    dictionary[(byte)x] = (byte)(p1 * x + p2);
                }
            }
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y,
                            out u, out v);

                        y = dictionary[(byte)y];

                        ClassColorModel.FromYuv(y, u, v, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1],
                            out buffer[pixelIndex]);
                    }
                }
            });
        }
        #endregion

        #region Log Correction

        public Bitmap LogCorrection(int k)
        {
            _function = string.Format("LogCorrection (k = {0})", k);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, cb, cr;
                        ClassColorModel.ToYCbCr(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y, out cb, out cr);
                        ClassColorModel.FromYCbCr(k * Math.Log(1 + y), cb, cr, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }

        #endregion

        #region Perfrect Reflector

        public Bitmap PerfectReflector()
        {
            _function = "PerfectReflector";
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int rMax = 0, gMax = 0, bMax = 0;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        if (buffer[pixelIndex + 2] > rMax) rMax = buffer[pixelIndex + 2];
                        if (buffer[pixelIndex + 1] > gMax) gMax = buffer[pixelIndex + 1];
                        if (buffer[pixelIndex] > bMax) bMax = buffer[pixelIndex];
                    }
                }
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        buffer[pixelIndex + 2] = GetByte(buffer[pixelIndex + 2] * 255d / rMax);
                        buffer[pixelIndex + 1] = GetByte(buffer[pixelIndex + 1] * 255d / gMax);
                        buffer[pixelIndex] = GetByte(buffer[pixelIndex] * 255d / bMax);
                    }
                }
            });
        }

        #endregion

        #region GrayScale

        public Bitmap AlgorithmGrayScale(Func<byte, byte, byte, byte> grayScaleType)
        {
            _function = "GrayScale";
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
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

        #endregion

        #region ChangeBrightness

        public Bitmap AlgorithmChangeBrightness(int k)
        {
            _function = string.Format("ChangeBrightness (k={0})", k);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                byte[] b = new byte[256];
                double lab = 0;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        lab += ClassColorModel.GetBrightness(buffer[pixelIndex + 2], buffer[pixelIndex + 1],
                            buffer[pixelIndex]);
                    }
                }

                lab /= Width * Height;
                double coef = 1.0 + k / 25.0;

                for (int i = 0; i < b.Length; i++)
                {
                    double delta = i - lab;
                    int temp = (int)(lab + coef * delta);

                    if (temp < 0)
                        temp = 0;

                    if (temp >= 255)
                        temp = 255;
                    b[i] = GetByte(temp);
                }

                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;

                        buffer[pixelIndex + 2] = b[buffer[pixelIndex + 2]];
                        buffer[pixelIndex + 1] = b[buffer[pixelIndex + 1]];
                        buffer[pixelIndex] = b[buffer[pixelIndex]];
                    }
                }
            });
        }

        #endregion

        #region Visualize Color Models

        public Bitmap VisualizeRgb(byte r, byte g, byte b)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int col = 0; col < bitmapData.Height; col++)
                {
                    int byteIndex = col * bitmapData.Stride;
                    for (int x = 0; x < bitmapData.Width; x++)
                    {
                        int pixelIndex = byteIndex + x * 3;

                        buffer[pixelIndex + 2] = (byte)(buffer[pixelIndex + 2] * r / 255);
                        buffer[pixelIndex + 1] = (byte)(buffer[pixelIndex + 1] * g / 255);
                        buffer[pixelIndex] = (byte)(buffer[pixelIndex] * b / 255);
                    }
                }
            });
        }

        #endregion

        #region Histogram

        public int[][] HistogramRgb(Image image)
        {
            int[][] hystogram = null;
            ReadBytes(image, ImageLockMode.ReadOnly, (buffer, bitmapData) =>
            {
                hystogram = HistogramRgb(buffer, bitmapData);
            });
            return hystogram;
        }

        private int[][] HistogramRgb(byte[] buffer, BitmapData bitmapData)
        {
            int[][] hystogram = new int[3][];
            for (int i = 0; i < hystogram.Length; i++)
            {
                hystogram[i] = new int[256];
            }
            for (int row = 0; row < bitmapData.Height; row++)
            {
                int byteIndex = row * bitmapData.Stride;
                for (int col = 0; col < bitmapData.Width; col++)
                {
                    int pixelIndex = byteIndex + col * 3;

                    hystogram[0][buffer[pixelIndex + 2]]++;
                    hystogram[1][buffer[pixelIndex + 1]]++;
                    hystogram[2][buffer[pixelIndex]]++;
                }
            }
            return hystogram;
        }

        public int[][] HistogramHsv(Image image)
        {
            var hystogram = new int[3][];
            hystogram[0] = new int[361];
            hystogram[1] = new int[101];
            hystogram[2] = new int[101];
            ReadBytes(image, ImageLockMode.ReadOnly, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double h, s, v;
                        ClassColorModel.ToHsv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out h,
                            out s, out v);

                        hystogram[0][GetInt(h)]++;
                        hystogram[1][GetInt(s * 100)]++;
                        hystogram[2][GetInt(v * 100)]++;
                    }
                }
            });
            return hystogram;
        }

        public int[][] HistogramYuv(Image image)
        {
            int[][] hystogram = null;
            ReadBytes(image, ImageLockMode.ReadOnly, (buffer, bitmapData) =>
            {
                hystogram = HistogramYuv(buffer, bitmapData);
            });
            return hystogram;
        }

        private int[][] HistogramYuv(byte[] buffer, BitmapData bitmapData)
        {
            int[][] hystogram = new int[3][];
            hystogram[0] = new int[256];
            hystogram[1] = new int[225];
            hystogram[2] = new int[315];
            for (int row = 0; row < bitmapData.Height; row++)
            {
                int byteIndex = row * bitmapData.Stride;
                for (int col = 0; col < bitmapData.Width; col++)
                {
                    int pixelIndex = byteIndex + col * 3;
                    double y, u, v;
                    ClassColorModel.ToYuv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y, out u,
                        out v);

                    hystogram[0][GetInt(y)]++;
                    hystogram[1][GetInt(u) + 112]++;
                    hystogram[2][GetInt(v) + 157]++;
                }
            }
            return hystogram;
        }

        #endregion

        #region Noise

        #region Additive Noise

        public Bitmap AdditiveNoiseRgb(int noiseLevel, bool[] components, int deviation)
        {
            /*_function = string.Format("AdditiveNoiseRgb (noiseLevel={0}, deviation={1})", noiseLevel, deviation);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    if (components[0])
                        buffer.SetPixel(pixelIndex + 2,
                            buffer[pixelIndex + 2] + Random.Next(deviation * 2 + 1) - deviation);
                    if (components[1])
                        buffer.SetPixel(pixelIndex + 1,
                            buffer[pixelIndex + 1] + Random.Next(deviation * 2 + 1) - deviation);
                    if (components[2])
                        buffer.SetPixel(pixelIndex, buffer[pixelIndex] + Random.Next(deviation * 2 + 1) - deviation);
                }
            });*/
            return null;
        }

        public Bitmap AdditiveNoiseHsv(int noiseLevel, bool[] components, int deviation)
        {
            /*_function = string.Format("AdditiveNoiseHsv (noiseLevel={0}, deviation={1})", noiseLevel, deviation);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double h, s, v;
                    ClassColorModel.ToHsv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out h, out s,
                        out v);

                    s *= 100;
                    v *= 100;

                    if (components[0]) h += Random.Next(deviation * 2 + 1) - deviation;
                    if (components[1]) s += Random.Next(deviation * 2 + 1) - deviation;
                    if (components[2]) v += Random.Next(deviation * 2 + 1) - deviation;

                    s /= 100d;
                    v /= 100d;

                    byte r, g, b;
                    ClassColorModel.FromHsv(h, s, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        public Bitmap AdditiveNoiseYuv(int noiseLevel, bool[] components, int deviation)
        {
            _function = string.Format("AdditiveNoiseYuv (noiseLevel={0}, deviation={1})", noiseLevel, deviation);
            /*return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double y, u, v;
                    ClassColorModel.ToYuv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y, out u,
                        out v);

                    if (components[0]) y += Random.Next(deviation * 2 + 1) - deviation;
                    if (components[1]) u += Random.Next(deviation * 2 + 1) - deviation;
                    if (components[2]) v += Random.Next(deviation * 2 + 1) - deviation;

                    byte r, g, b;
                    ClassColorModel.FromYuv(y, u, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        #endregion

        #region Multiplicative Noise

        public Bitmap MultiplicativeNoiseRgb(int noiseLevel, bool[] components, int from, int to)
        {
            /*_function = string.Format("MultiplicativeNoiseRgb (noiseLevel={0}, [{1};{2}])", noiseLevel, from, to);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    if (components[0])
                        buffer.SetPixel(pixelIndex + 2, GetInt(buffer[pixelIndex + 2] * Random.Next(from, to) / 100f));
                    if (components[1])
                        buffer.SetPixel(pixelIndex + 1, GetInt(buffer[pixelIndex + 1] * Random.Next(from, to) / 100f));
                    if (components[2])
                        buffer.SetPixel(pixelIndex, GetInt(buffer[pixelIndex] * Random.Next(from, to) / 100f));
                }
            });*/
            return null;
        }

        public Bitmap MultiplicativeNoiseHsv(int noiseLevel, bool[] components, int from, int to)
        {
           /* _function = string.Format("MultiplicativeNoiseHsv (noiseLevel={0}, [{1};{2}])", noiseLevel, from, to);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double h, s, v;
                    ClassColorModel.ToHsv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out h, out s,
                        out v);

                    s *= 100;
                    v *= 100;

                    if (components[0]) h *= Random.Next(from, to) / 100f;
                    if (components[1]) s *= Random.Next(from, to) / 100f;
                    if (components[2]) v *= Random.Next(from, to) / 100f;

                    s /= 100d;
                    v /= 100d;

                    byte r, g, b;
                    ClassColorModel.FromHsv(h, s, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        public Bitmap MultiplicativeNoiseYuv(int noiseLevel, bool[] components, int from, int to)
        {
           /* _function = string.Format("MultiplicativeNoiseYuv (noiseLevel={0}, [{1};{2}])", noiseLevel, from, to);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double y, u, v;
                    ClassColorModel.ToYuv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y, out u,
                        out v);

                    if (components[0]) y *= Random.Next(from, to) / 100f;
                    if (components[1]) u *= Random.Next(from, to) / 100f;
                    if (components[2]) v *= Random.Next(from, to) / 100f;

                    byte r, g, b;
                    ClassColorModel.FromYuv(y, u, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        #endregion

        #region Implulse Noise

        public Bitmap ImplulseNoiseRgb(int noiseLevel, bool[] components, int k)
        {
            _function = string.Format("ImplulseNoiseRgb (noiseLevel={0}, k={1})", noiseLevel, k);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();
                    byte color = (byte)(Random.Next(100) > k ? 0 : 255);

                    if (components[0]) buffer[pixelIndex + 2] = color;
                    if (components[1]) buffer[pixelIndex + 1] = color;
                    if (components[2]) buffer[pixelIndex] = color;
                }
            });
        }

        public Bitmap ImplulseNoiseHsv(int noiseLevel, bool[] components, int k)
        {
            /*_function = string.Format("ImplulseNoiseHsv (noiseLevel={0}, k={1})", noiseLevel, k);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double h, s, v;
                    ClassColorModel.ToHsv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out h, out s,
                        out v);

                    s *= 100;
                    v *= 100;

                    int color = Random.Next(100) > k ? 180 : -180;

                    if (components[0] || components[1] || components[2]) h += color;

                    s /= 100d;
                    v /= 100d;

                    byte r, g, b;
                    ClassColorModel.FromHsv(h, s, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        public Bitmap ImplulseNoiseYuv(int noiseLevel, bool[] components, int k)
        {
           /* _function = string.Format("ImplulseNoiseYuv (noiseLevel={0}, k={1})", noiseLevel, k);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int count = _pixelCount * noiseLevel / 100;

                for (int i = 0; i < count; i++)
                {
                    int pixelIndex = GetRandomPixel();

                    double y, u, v;
                    ClassColorModel.ToYuv(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex], out y, out u,
                        out v);

                    var impulse = Random.Next(100) > k ? 0 : 255;

                    if (components[0]) y = impulse;
                    if (components[1]) u = impulse;
                    if (components[2]) v = impulse;

                    byte r, g, b;
                    ClassColorModel.FromYuv(y, u, v, out r, out g, out b);
                    if (components[0]) buffer.SetPixel(pixelIndex + 2, r);
                    if (components[1]) buffer.SetPixel(pixelIndex + 1, g);
                    if (components[2]) buffer.SetPixel(pixelIndex, b);
                }
            });*/
            return null;
        }

        #endregion

        #endregion

        #region Filters

        #region LinearAverage

        public Bitmap FilterLinearAverageRgb(int radius, bool[] colorModelComponents)
        {
            _function = string.Format("LinearAverageRgb (radius={0})", radius);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int n = GetFilterSize(radius);
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        int r = _original[pixelIndex + 2], g = _original[pixelIndex + 1], b = _original[pixelIndex];
                        double r1 = 0, g1 = 0, b1 = 0;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            for (int j = -radius; j <= radius; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;

                                if (colorModelComponents[0]) r1 += 1f / _original[row2 + col2 + 2];
                                if (colorModelComponents[1]) g1 += 1f / _original[row2 + col2 + 1];
                                if (colorModelComponents[2]) b1 += 1f / _original[row2 + col2];
                            }
                        }

                        if (colorModelComponents[0]) r = (int)(n / r1);
                        if (colorModelComponents[1]) g = (int)(n / g1);
                        if (colorModelComponents[2]) b = (int)(n / b1);

                        buffer[pixelIndex + 2] = (byte)ClassColorModel.GetValue(r, 0, 255);
                        buffer[pixelIndex + 1] = (byte)ClassColorModel.GetValue(g, 0, 255);
                        buffer[pixelIndex] = (byte)ClassColorModel.GetValue(b, 0, 255);
                    }
                }
            });
        }

        public Bitmap FilterLinearAverageRgbRecursive(int radius, bool[] colorModelComponents)
        {
            _function = string.Format("LinearAverageRgbRecursive (radius={0})", radius);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int n = GetFilterSize(radius);
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;

                    //Calculate 
                    byte r = _original[byteIndex + 2], g = _original[byteIndex + 1], b = _original[byteIndex];
                    double r1 = 0, g1 = 0, b1 = 0;
                    for (int i = -radius; i <= radius; i++)
                    {
                        var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                        for (int j = -radius; j <= radius; j++)
                        {
                            var col2 = ClassColorModel.GetValue(j, 0, Width - 1) * 3;

                            if (colorModelComponents[0]) r1 += GetInvertColor(_original[row2 + col2 + 2]);
                            if (colorModelComponents[1]) g1 += GetInvertColor(_original[row2 + col2 + 1]);
                            if (colorModelComponents[2]) b1 += GetInvertColor(_original[row2 + col2]);
                        }
                    }
                    if (colorModelComponents[0]) r = GetInvertColor(r1, n);
                    if (colorModelComponents[1]) g = GetInvertColor(g1, n);
                    if (colorModelComponents[2]) b = GetInvertColor(b1, n);

                    buffer[byteIndex + 2] = r;
                    buffer[byteIndex + 1] = g;
                    buffer[byteIndex] = b;

                    for (int col = 1; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;

                        r = _original[pixelIndex + 2];
                        g = _original[pixelIndex + 1];
                        b = _original[pixelIndex];

                        //Substract left line of pixels
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            var col2 = ClassColorModel.GetValue(col - radius - 1, 0, Width - 1) * 3;
                            if (colorModelComponents[0]) r1 -= GetInvertColor(_original[row2 + col2 + 2]);
                            if (colorModelComponents[1]) g1 -= GetInvertColor(_original[row2 + col2 + 1]);
                            if (colorModelComponents[2]) b1 -= GetInvertColor(_original[row2 + col2]);
                        }
                        //Add right line of pixels
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            var col2 = ClassColorModel.GetValue(col + radius, 0, Width - 1) * 3;
                            if (colorModelComponents[0]) r1 += GetInvertColor(_original[row2 + col2 + 2]);
                            if (colorModelComponents[1]) g1 += GetInvertColor(_original[row2 + col2 + 1]);
                            if (colorModelComponents[2]) b1 += GetInvertColor(_original[row2 + col2]);
                        }

                        if (colorModelComponents[0]) r = GetInvertColor(r1, n);
                        if (colorModelComponents[1]) g = GetInvertColor(g1, n);
                        if (colorModelComponents[2]) b = GetInvertColor(b1, n);

                        buffer[pixelIndex + 2] = r;
                        buffer[pixelIndex + 1] = g;
                        buffer[pixelIndex] = b;
                    }
                }
            });
        }

        public Bitmap FilterLinearAverageYuv(int radius, bool[] colorModelComponents)
        {
            _function = string.Format("LinearAverageYuv (radius={0})", radius);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int n = GetFilterSize(radius);
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out y, out u, out v);
                        double y1 = 0, u1 = 0, v1 = 0;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;

                            for (int j = -radius; j <= radius; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;

                                double y2, u2, v2;
                                ClassColorModel.ToYuv(_original[row2 + col2 + 2], _original[row2 + col2 + 1],
                                    _original[row2 + col2], out y2, out u2, out v2);

                                if (colorModelComponents[0]) y1 += 1 / y2;
                                if (colorModelComponents[1]) u1 += 1 / u2;
                                if (colorModelComponents[2]) v1 += 1 / v2;

                            }
                        }

                        if (colorModelComponents[0]) y = n / y1;
                        if (colorModelComponents[1]) u = n / u1;
                        if (colorModelComponents[2]) v = n / v1;

                        ClassColorModel.FromYuv(y, u, v, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }

        public Bitmap FilterLinearAverageYuvRecursive(int radius, bool[] colorModelComponents)
        {
            _function = string.Format("LinearAverageYuvRecursive (radius={0})", radius);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int n = GetFilterSize(radius);
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out y, out u, out v);
                        double y1 = 0, u1 = 0, v1 = 0;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;

                            for (int j = -radius; j <= radius; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;

                                double y2, u2, v2;
                                ClassColorModel.ToYuv(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2], out y2, out u2, out v2);

                                if (colorModelComponents[0]) y1 += 1 / y2;
                                if (colorModelComponents[1]) u1 += 1 / u2;
                                if (colorModelComponents[2]) v1 += 1 / v2;
                            }
                        }

                        if (colorModelComponents[0]) y = n / y1;
                        if (colorModelComponents[1]) u = n / u1;
                        if (colorModelComponents[2]) v = n / v1;

                        ClassColorModel.FromYuv(y, u, v, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }

        #endregion

        #region Filter Average Point

        public Bitmap FilterAveragePointRgb2D(int height, int width, bool[] colorModelComponents)
        {
            height /= 2;
            width /= 2;
            _function = string.Format("AveragePointRgb2D ({0}x{1})", height, width);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        int rMin = 255, gMin = 255, bMin = 255;
                        int rMax = 0, gMax = 0, bMax = 0;
                        for (int i = -height; i <= height; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            for (int j = -width; j <= width; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;

                                if (colorModelComponents[0])
                                {
                                    if (_original[row2 + col2 + 2] > rMax)
                                    {
                                        rMax = _original[row2 + col2 + 2];
                                    }
                                    if (_original[row2 + col2 + 2] < rMin)
                                    {
                                        rMin = _original[row2 + col2 + 2];
                                    }
                                }
                                if (colorModelComponents[1])
                                {
                                    if (_original[row2 + col2 + 1] > gMax)
                                    {
                                        gMax = _original[row2 + col2 + 1];
                                    }
                                    if (_original[row2 + col2 + 1] < gMin)
                                    {
                                        gMin = _original[row2 + col2 + 1];
                                    }
                                }
                                if (colorModelComponents[2])
                                {
                                    if (_original[row2 + col2] > bMax)
                                    {
                                        bMax = _original[row2 + col2];
                                    }
                                    if (_original[row2 + col2] < bMin)
                                    {
                                        bMin = _original[row2 + col2];
                                    }
                                }
                            }
                        }

                        if (colorModelComponents[0]) buffer[pixelIndex + 2] = (byte)((rMax + rMin) / 2);
                        if (colorModelComponents[1]) buffer[pixelIndex + 1] = (byte)((gMax + gMin) / 2);
                        if (colorModelComponents[2]) buffer[pixelIndex] = (byte)((bMax + bMin) / 2);
                    }
                }
            });
        }

        public Bitmap FilterAveragePointYuv2D(int height, int width, bool[] colorModelComponents)
        {
            _function = string.Format("AveragePointYuv2D ({0}x{1})", height, width);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                height /= 2;
                width /= 2;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out y,
                            out u, out v);
                        double yMin = Double.MaxValue, uMin = Double.MaxValue, vMin = Double.MaxValue;
                        double yMax = Double.MinValue, uMax = Double.MinValue, vMax = Double.MinValue;
                        for (int i = -height; i <= height; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            for (int j = -width; j <= width; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;

                                double y1, u1, v1;
                                ClassColorModel.ToYuv(_original[row2 + col2 + 2], _original[row2 + col2 + 1],
                                    _original[row2 + col2], out y1, out u1, out v1);

                                if (colorModelComponents[0])
                                {
                                    if (y1 > yMax)
                                    {
                                        yMax = y1;
                                    }
                                    if (y1 < yMin)
                                    {
                                        yMin = y1;
                                    }
                                }
                                if (colorModelComponents[1])
                                {
                                    if (u1 > uMax)
                                    {
                                        uMax = u1;
                                    }
                                    if (u1 < uMin)
                                    {
                                        uMin = u1;
                                    }
                                }
                                if (colorModelComponents[2])
                                {
                                    if (v1 > vMax)
                                    {
                                        vMax = v1;
                                    }
                                    if (v1 < vMin)
                                    {
                                        vMin = v1;
                                    }
                                }
                            }
                        }

                        if (colorModelComponents[0]) y = (yMax + yMin) / 2;
                        if (colorModelComponents[1]) u = (uMax + uMin) / 2;
                        if (colorModelComponents[2]) v = (vMax + vMin) / 2;

                        ClassColorModel.FromYuv(y, u, v, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1],
                            out buffer[pixelIndex]);
                    }
                }
            });
        }

        #endregion

        #region Gaus Filter

        public Bitmap FilterGaus(int radius, double sigma)
        {
            _function = string.Format("Gaus (radius={0}, sigma={1})", radius, sigma);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                FilterGaus(radius, sigma, buffer, bitmapData);
            });
        }

        public void FilterGaus(int radius, double sigma, byte[] buffer, BitmapData bitmapData)
        {
            var gaussKernel = GetGausKernel(radius, sigma);
            for (int row = 0; row < bitmapData.Height; row++)
            {
                int byteIndex = row * bitmapData.Stride;
                for (int col = 0; col < bitmapData.Width; col++)
                {
                    int pixelIndex = byteIndex + col * 3;

                    double rSum = 0, gSum = 0, bSum = 0, kSum = 0;
                    int k = 0;
                    for (int i = -radius; i <= radius; i++)
                    {
                        var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;

                        for (int j = -radius; j <= radius; j++)
                        {
                            var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;
                            var kernel = gaussKernel[k];

                            rSum += _original[row2 + col2 + 2] * kernel;
                            gSum += _original[row2 + col2 + 1] * kernel;
                            bSum += _original[row2 + col2] * kernel;

                            kSum += kernel;
                            k++;
                        }
                    }
                    buffer[pixelIndex + 2] = GetAv(rSum, kSum);
                    buffer[pixelIndex + 1] = GetAv(gSum, kSum);
                    buffer[pixelIndex] = GetAv(bSum, kSum);
                }
            }
        }

        #endregion

        #region Single Scale Retinex


        public Bitmap FilterSingleScaleRetinex(int radius, double sigma)
        {
            _function = string.Format("SingleScaleRetinex (radius={0}, sigma={1})", radius, sigma);
            var kernelSize = 2 * radius + 1;
            var sum = 0d;
            int k = 0;
            double kk = 1d / (2 * Math.PI * sigma * sigma);
            var gaussKernel = new double[kernelSize * kernelSize];
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    var val = kk * Math.Exp(-(i * i + j * j) / (2 * sigma * sigma));
                    gaussKernel[k] = val;
                    sum += val;
                    k++;
                }
            }

            for (int i = 0; i < gaussKernel.Length; i++) gaussKernel[i] /= sum;

            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out y, out u, out v);
                        double val = 0;
                        double kSum = 0;
                        k = 0;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;

                            for (int j = -radius; j <= radius; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;
                                var kernel = gaussKernel[k];
                                val += ClassColorModel.GetBrightness(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2]) * kernel;
                                kSum += kernel;
                                k++;
                            }
                        }
                        double p = GetAv(val, kSum);
                        y = 255 * Math.Log(y / p) + 127.5;
                        ClassColorModel.FromYuv(y, u, v, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }

        private byte GetAv(double sum, double k)
        {
            sum /= k;
            if (sum < 0) sum = 0;
            else if (sum > 255) sum = 255;
            return (byte)sum;
        }

        #endregion

        #region UnsharpMask

        /* Фильтр повышения резкости изображения (технология UnsharpMask) 
         * с заданием вида размытия и ее маски с возможностью обработки при установки порога.*/

        public Bitmap FilterUnsharpMask(int radius, bool meanFilter, double amount = 1, int bias = 0, int threshold = 5)
        {
            _function = string.Format("UnsharpMask (amount={0}, threshold={1}, radius={2}, {3})", amount, threshold, radius, meanFilter ? "Mean" : "Gaus");
            double[] filterMatrix;
            if (meanFilter)
            {
                int size = GetFilterSize(radius);
                filterMatrix = new double[size];
                for (int i = 0; i < filterMatrix.Length; i++)
                {
                    filterMatrix[i] = 1;
                }
            }
            else
            {
                filterMatrix = GetGausKernel(radius, 5);
            }

            return FilterUnsharpMask(filterMatrix, amount, bias, threshold);
        }

        public Bitmap FilterUnsharpMask(double[] filterMatrix, double amount = 1, int bias = 0, int threshold = 5)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                var filter = buffer.ToArray();
                ConvolutionFilter(filter, filterMatrix, amount, bias);

                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;

                        double r = (buffer[pixelIndex + 2] - filter[pixelIndex + 2]) * 1;
                        double g = (buffer[pixelIndex + 1] - filter[pixelIndex + 1]) * 1;
                        double b = (buffer[pixelIndex] - filter[pixelIndex]) * 1;
                        if (ClassColorModel.GetBrightness(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex]) > threshold)
                        {
                            buffer[pixelIndex + 2] = (byte)ClassColorModel.GetValue(buffer[pixelIndex + 2] + r, 0, 255);
                            buffer[pixelIndex + 1] = (byte)ClassColorModel.GetValue(buffer[pixelIndex + 1] + g, 0, 255);
                            buffer[pixelIndex] = (byte)ClassColorModel.GetValue(buffer[pixelIndex] + b, 0, 255);
                        }
                    }
                }
            });
        }


        public Bitmap ConvolutionFilter(double[] filterMatrix, double amount = 1, int bias = 0)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                ConvolutionFilter(buffer, filterMatrix, amount, bias);
            });
        }

        void ConvolutionFilter(byte[] buffer, double[] filterMatrix, double amount = 1, int bias = 0)
        {
            byte[] original = buffer.ToArray();

            int filterWidth = (int)Math.Sqrt(filterMatrix.Length);
            int filterOffset = (filterWidth - 1) / 2;

            for (int row = 0; row < Height; row++)
            {
                int byteIndex = row * Stride;
                for (int col = 0; col < Width; col++)
                {
                    int pixelIndex = byteIndex + col * 3;

                    double b = 0;
                    double g = 0;
                    double r = 0;
                    for (int i = -filterOffset; i <= filterOffset; i++)
                    {
                        var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * Stride;
                        for (int j = -filterOffset; j <= filterOffset; j++)
                        {
                            var position = row2 + ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;
                            var filter = filterMatrix[(i + filterOffset) * filterWidth + j + filterOffset];
                            r += original[position + 2] * filter;
                            g += original[position + 1] * filter;
                            b += original[position] * filter;
                        }
                    }

                    buffer[pixelIndex + 2] = (byte)ClassColorModel.GetValue(amount * r + bias, 0, 255);
                    buffer[pixelIndex + 1] = (byte)ClassColorModel.GetValue(amount * g + bias, 0, 255);
                    buffer[pixelIndex] = (byte)ClassColorModel.GetValue(amount * b + bias, 0, 255);
                }
            }
        }
        #endregion

        #endregion

        #region Segmentation

        #region Binarization

        public Bitmap SegmentationBinarizationGlobal(int from, int to, bool invert)
        {
            _function = string.Format("Binarization Global (from={0}, to={1})", from, to);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                byte black = invert ? White : Black;
                byte white = invert ? Black : White;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        double y = ClassColorModel.GetBrightness(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex]);

                        byte bin = y >= from & y <= to ? black : white;

                        buffer[pixelIndex + 2] = bin;
                        buffer[pixelIndex + 1] = bin;
                        buffer[pixelIndex] = bin;
                    }
                }
            });
        }

        public Bitmap SegmentationBinarizationBernsen(int radius, int threshold, bool invert)
        {
            _function = string.Format("Binarization Bernsen (radius={0}, threshold={1})", radius, threshold);
            byte black = invert ? White : Black;
            byte white = invert ? Black : White;
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        byte max = Black;
                        byte min = White;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            for (int j = -radius; j <= radius; j++)
                            {
                                var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;
                                var y = (byte)ClassColorModel.GetBrightness(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2]);
                                if (y > max) max = y;
                                if (y < min) min = y;
                            }
                        }
                        byte bin = (max - min) / 2 < threshold ? black : white;
                        buffer[pixelIndex + 2] = bin;
                        buffer[pixelIndex + 1] = bin;
                        buffer[pixelIndex] = bin;
                    }
                }
            });
        }
        #endregion

        #region Morphologization

        public Bitmap SegmentationMorphologizationDilation(int radius, int structureElement)
        {
            _function = string.Format("MorphDilation (radius={0}, structElem={1})", radius, structureElement);
            return SegmentationMorphologizationCore(radius, structureElement, Black, (y, val) => y > val);
        }

        public Bitmap SegmentationMorphologizationErosion(int radius, int structureElement)
        {
            _function = string.Format("MorphErosion (radius={0}, structElem={1})", radius, structureElement);
            return SegmentationMorphologizationCore(radius, structureElement, White, (y, val) => y < val);
        }

        private Bitmap SegmentationMorphologizationCore(int radius, int structureElement, byte initialValue, Func<byte, byte, bool> thresholdFunc)
        {
            bool[] kernel = GetMorphologizationKernel(structureElement, radius);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        byte val = initialValue;
                        int k = 0;
                        for (int i = -radius; i <= radius; i++)
                        {
                            var row2 = ClassColorModel.GetValue(row + i, 0, Height - 1) * bitmapData.Stride;
                            for (int j = -radius; j <= radius; j++)
                            {
                                if (kernel[k])
                                {
                                    var col2 = ClassColorModel.GetValue(col + j, 0, Width - 1) * 3;
                                    var y = (byte)ClassColorModel.GetBrightness(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2]);
                                    if (thresholdFunc(y, val)) val = y;
                                }
                                k++;
                            }
                        }
                        var bin = val > 128 ? White : Black;
                        buffer[pixelIndex + 2] = bin;
                        buffer[pixelIndex + 1] = bin;
                        buffer[pixelIndex] = bin;
                    }
                }
            });
        }

        #endregion

        #region Controur representation

        #region Roberson
        public Bitmap SegmentationContourRobertsonRgb(int threshold, double gain, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Robertson Rgb (threshold={0}, gain={1})", threshold, gain);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var row2 = ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = row1 + col * 3;
                        var col1 = col * 3;
                        var col2 = ClassColorModel.GetValue(col + 1, 0, Width - 1) * 3;

                        if (colorModelComponents[0])
                        {
                            var rob = OperatorRobertson(_original[pixelIndex + 2], _original[row2 + col2 + 2], _original[row1 + col2 + 2], _original[row2 + col1 + 2]);
                            buffer[pixelIndex + 2] = rob < threshold ? Black : (byte)ClassColorModel.GetValue(rob * gain, 0, 255);
                        }
                        if (colorModelComponents[1])
                        {
                            var rob = OperatorRobertson(_original[pixelIndex + 1], _original[row2 + col2 + 1], _original[row1 + col2 + 1], _original[row2 + col1 + 1]);
                            buffer[pixelIndex + 1] = rob < threshold ? Black : (byte)ClassColorModel.GetValue(rob * gain, 0, 255);
                        }
                        if (colorModelComponents[2])
                        {
                            var rob = OperatorRobertson(_original[pixelIndex], _original[row2 + col2], _original[row1 + col2], _original[row2 + col1]);
                            buffer[pixelIndex] = rob < threshold ? Black : (byte)ClassColorModel.GetValue(rob * gain, 0, 255);
                        }
                    }
                }
            });
        }

        public Bitmap SegmentationContourRobertsonHsv(int threshold, double gain, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Robertson HSV (threshold={0}, gain={1})", threshold, gain);
            var svThreshold = threshold / 100d;
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var row2 = ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = row1 + col * 3;
                        var col1 = col * 3;
                        var col2 = ClassColorModel.GetValue(col + 1, 0, Width - 1) * 3;

                        double h1, h2, h3, h4;
                        double s1, s2, s3, s4;
                        double v1, v2, v3, v4;
                        ClassColorModel.ToHsv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out h1, out s1, out v1);
                        ClassColorModel.ToHsv(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2], out h2, out s2, out v2);
                        ClassColorModel.ToHsv(_original[row1 + col2 + 2], _original[row1 + col2 + 1], _original[row1 + col2], out h3, out s3, out v3);
                        ClassColorModel.ToHsv(_original[row2 + col1 + 2], _original[row2 + col1 + 1], _original[row2 + col1], out h4, out s4, out v4);

                        if (colorModelComponents[0])
                        {
                            var rob = OperatorRobertson(h1, h2, h3, h4);
                            h1 = rob < threshold ? 0 : rob * gain;
                        }
                        if (colorModelComponents[1])
                        {
                            var rob = OperatorRobertson(s1, s2, s3, s4);
                            s1 = rob < svThreshold ? 0 : rob * gain;
                        }
                        if (colorModelComponents[2])
                        {
                            var rob = OperatorRobertson(v1, v2, v3, v4);
                            v1 = rob < svThreshold ? 0 : rob * gain;
                        }

                        ClassColorModel.FromHsv(h1, s1, v1, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }

        public Bitmap SegmentationContourRobertsonYuv(int threshold, double gain, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Robertson Yuv (threshold={0}, gain={1})", threshold, gain);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var row2 = ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = row1 + col * 3;
                        var col1 = col * 3;
                        var col2 = ClassColorModel.GetValue(col + 1, 0, Width - 1) * 3;
                        double y1, y2, y3, y4;
                        double u1, u2, u3, u4;
                        double v1, v2, v3, v4;
                        ClassColorModel.ToYuv(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex], out y1, out u1, out v1);
                        ClassColorModel.ToYuv(_original[row2 + col2 + 2], _original[row2 + col2 + 1], _original[row2 + col2], out y2, out u2, out v2);
                        ClassColorModel.ToYuv(_original[row1 + col2 + 2], _original[row1 + col2 + 1], _original[row1 + col2], out y3, out u3, out v3);
                        ClassColorModel.ToYuv(_original[row2 + col1 + 2], _original[row2 + col1 + 1], _original[row2 + col1], out y4, out u4, out v4);

                        if (colorModelComponents[0])
                        {
                            var rob = OperatorRobertson(y1, y2, y3, y4);
                            y1 = rob < threshold ? 0 : rob * gain;
                        }
                        if (colorModelComponents[1])
                        {
                            var rob = OperatorRobertson(u1, u2, u3, u4);
                            u1 = rob < threshold ? 0 : rob * gain;
                        }
                        if (colorModelComponents[2])
                        {
                            var rob = OperatorRobertson(v1, v2, v3, v4);
                            v1 = rob < threshold ? 0 : rob * gain;
                        }

                        ClassColorModel.FromYuv(y1, u1, v1, out buffer[pixelIndex + 2], out buffer[pixelIndex + 1], out buffer[pixelIndex]);
                    }
                }
            });
        }
        #endregion

        #region Sobel

        public Bitmap SegmentationContourSobelRgb(int threshold, double gain, double[][] gx, double[][] gy, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Sobel Rgb (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionRgb(threshold, gain, gx, gy, colorModelComponents);
        }

        public Bitmap SegmentationContourSobelHsv(int threshold, double gain, double[][] gx, double[][] gy, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Sobel Hsv (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionHsv(threshold, gain, gx, gy, colorModelComponents);
        }

        public Bitmap SegmentationContourSobelYuv(int threshold, double gain, double[][] gx, double[][] gy, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Sobel Yuv (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionYuv(threshold, gain, gx, gy, colorModelComponents);
        }

        #endregion

        #region Laplas
        public Bitmap SegmentationContourLaplasRgb(int threshold, double gain, double[][] g, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Laplas Rgb (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionRgb(threshold, gain, g, g, colorModelComponents);
        }

        public Bitmap SegmentationContourLaplasHsv(int threshold, double gain, double[][] g, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Laplas Hsv (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionHsv(threshold, gain, g, g, colorModelComponents);
        }

        public Bitmap SegmentationContourLaplasYuv(int threshold, double gain, double[][] g, bool[] colorModelComponents)
        {
            _function = string.Format("Segmentation Laplas Yuv (threshold={0}, gain={1})", threshold, gain);
            return SegmentationContourConvolutionYuv(threshold, gain, g, g, colorModelComponents);
        }
        #endregion

        #region ContourConvolution

        public Bitmap SegmentationContourConvolutionRgb(int threshold, double gain, double[][] matrixGx, double[][] matrixGy, bool[] colorModelComponents)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var rows = new[]{
                        ClassColorModel.GetValue(row - 1, 0, Height - 1) * bitmapData.Stride,
                        row1, 
                        ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride
                    };
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int index = row1 + col * 3;

                        double sum1X = 0, sum1Y = 0;
                        double sum2X = 0, sum2Y = 0;
                        double sum3X = 0, sum3Y = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                var p = rows[i] + ClassColorModel.GetValue(col + j - 1, 0, Width - 1) * 3;
                                var gX = matrixGx[i][j];
                                var gY = matrixGy[i][j];
                                if (gX == 0 && gY == 0) continue;
                                if (colorModelComponents[0])
                                {
                                    sum1X += _original[p + 2] * gX;
                                    sum1Y += _original[p + 2] * gY;
                                }
                                if (colorModelComponents[1])
                                {
                                    sum2X += _original[p + 1] * gX;
                                    sum2Y += _original[p + 1] * gY;
                                }
                                if (colorModelComponents[2])
                                {
                                    sum3X += _original[p] * gX;
                                    sum3Y += _original[p] * gY;
                                }
                            }
                        }

                        if (colorModelComponents[0])
                        {
                            buffer[index + 2] = (byte)ClassColorModel.GetValue(OperatorSobel(threshold, gain, sum1X, sum1Y), 0, 255);
                        }
                        if (colorModelComponents[1])
                        {
                            buffer[index + 1] = (byte)ClassColorModel.GetValue(OperatorSobel(threshold, gain, sum2X, sum2Y), 0, 255);
                        }
                        if (colorModelComponents[2])
                        {
                            buffer[index] = (byte)ClassColorModel.GetValue(OperatorSobel(threshold, gain, sum3X, sum3Y), 0, 255);
                        }
                    }
                }
            });
        }

        public Bitmap SegmentationContourConvolutionHsv(int threshold, double gain, double[][] matrixGx, double[][] matrixGy, bool[] colorModelComponents)
        {
            var svThreshold = threshold / 100;
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var rows = new[]{
                        ClassColorModel.GetValue(row - 1, 0, Height - 1) * bitmapData.Stride,
                        row1, 
                        ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride
                    };
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int index = row1 + col * 3;
                        double h, s, v;
                        ClassColorModel.ToHsv(_original[index + 2], _original[index + 1], _original[index], out h, out s, out v);
                        double sum1X = 0, sum1Y = 0;
                        double sum2X = 0, sum2Y = 0;
                        double sum3X = 0, sum3Y = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                var p = rows[i] + ClassColorModel.GetValue(col + j - 1, 0, Width - 1) * 3;
                                double c1, c2, c3;
                                ClassColorModel.ToHsv(_original[p + 2], _original[p + 1], _original[p], out c1, out c2, out c3);
                                var gX = matrixGx[i][j];
                                var gY = matrixGy[i][j];
                                if (gX == 0 && gY == 0) continue;
                                if (colorModelComponents[0])
                                {
                                    sum1X += c1 * gX;
                                    sum1Y += c1 * gY;
                                }
                                if (colorModelComponents[1])
                                {
                                    sum2X += c2 * gX;
                                    sum2Y += c2 * gY;
                                }
                                if (colorModelComponents[2])
                                {
                                    sum3X += c3 * gX;
                                    sum3Y += c3 * gY;
                                }
                            }
                        }

                        if (colorModelComponents[0])
                        {
                            h = OperatorSobel(threshold, gain, sum1X, sum1Y);
                        }
                        if (colorModelComponents[1])
                        {
                            s = OperatorSobel(svThreshold, gain, sum2X, sum2Y);
                        }
                        if (colorModelComponents[2])
                        {
                            v = OperatorSobel(svThreshold, gain, sum3X, sum3Y);
                        }

                        ClassColorModel.FromHsv(h, s, v, out buffer[index + 2], out buffer[index + 1], out buffer[index]);
                    }
                }
            });
        }

        public Bitmap SegmentationContourConvolutionYuv(int threshold, double gain, double[][] matrixGx, double[][] matrixGy, bool[] colorModelComponents)
        {
            var svThreshold = threshold / 100;
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int row1 = row * bitmapData.Stride;
                    var rows = new[]{
                        ClassColorModel.GetValue(row - 1, 0, Height - 1) * bitmapData.Stride,
                        row1, 
                        ClassColorModel.GetValue(row + 1, 0, Height - 1) * bitmapData.Stride
                    };
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int index = row1 + col * 3;
                        double y, u, v;
                        ClassColorModel.ToYuv(_original[index + 2], _original[index + 1], _original[index], out y, out u, out v);
                        double sum1X = 0, sum1Y = 0;
                        double sum2X = 0, sum2Y = 0;
                        double sum3X = 0, sum3Y = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                var p = rows[i] + ClassColorModel.GetValue(col + j - 1, 0, Width - 1) * 3;
                                double c1, c2, c3;
                                ClassColorModel.ToYuv(_original[p + 2], _original[p + 1], _original[p], out c1, out c2, out c3);
                                var gX = matrixGx[i][j];
                                var gY = matrixGy[i][j];
                                if (gX == 0 && gY == 0) continue;
                                if (colorModelComponents[0])
                                {
                                    sum1X += c1 * gX;
                                    sum1Y += c1 * gY;
                                }
                                if (colorModelComponents[1])
                                {
                                    sum2X += c2 * gX;
                                    sum2Y += c2 * gY;
                                }
                                if (colorModelComponents[2])
                                {
                                    sum3X += c3 * gX;
                                    sum3Y += c3 * gY;
                                }
                            }
                        }

                        if (colorModelComponents[0])
                        {
                            y = OperatorSobel(threshold, gain, sum1X, sum1Y);
                        }
                        if (colorModelComponents[1])
                        {
                            u = OperatorSobel(svThreshold, gain, sum2X, sum2Y);
                        }
                        if (colorModelComponents[2])
                        {
                            v = OperatorSobel(svThreshold, gain, sum3X, sum3Y);
                        }

                        ClassColorModel.FromYuv(y, u, v, out buffer[index + 2], out buffer[index + 1], out buffer[index]);
                    }
                }
            });
        }
        #endregion

        #region Operators
        private double OperatorRobertson(double g1, double g2, double g3, double g4)
        {
            return Math.Abs(g1 - g2) * Math.Abs(g3 - g4);
        }

        private double OperatorSobel(double threshold, double gain, double x, double y)
        {
            var sum = Math.Abs(x) + Math.Abs(y);
            return sum < threshold ? Black : sum * gain;
        }
        #endregion

        #endregion

        public Bitmap SegmentationRegionsGrowing(double threshold)
        {
            _function = string.Format("Regions Growing (threshold={0})", threshold);
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int[][] labels = new int[bitmapData.Height][];
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i] = new int[bitmapData.Width];
                }
                var regions = new Dictionary<int, List<Point>>();
                var brightnessDictionary = new Dictionary<int, double>();
                int labelNumber = 0;
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int index = row * Stride + col * 3;
                        var brightness = ClassColorModel.GetBrightness(buffer[index + 2], buffer[index + 1], buffer[index]);
                        double brightnessTop;
                        if (row > 0)
                        {
                            var label = labels[row - 1][col];
                            brightnessTop = brightnessDictionary[label] / regions[label].Count;
                        }
                        else
                        {
                            brightnessTop = 1000;
                        }

                        double brightnessLeft;
                        if (col > 0)
                        {
                            var label = labels[row][col - 1];
                            brightnessLeft = brightnessDictionary[label] / regions[label].Count;
                        }
                        else
                        {
                            brightnessLeft = 1000;
                        }

                        if (row > 0 && col > 0 && Math.Abs(brightness - brightnessTop) < threshold && Math.Abs(brightness - brightnessLeft) < threshold)
                        {
                            var label1 = labels[row][col - 1];
                            var label2 = labels[row - 1][col];
                            labels[row][col] = label1;
                            regions[label1].Add(new Point(col, row));
                            brightnessDictionary[label1] += brightness;
                            if (label1 != label2)
                            {
                                regions[label1].AddRange(regions[label2]);
                                brightnessDictionary[label1] += brightnessDictionary[label2];
                                foreach (var point in regions[label2])
                                {
                                    labels[point.Y][point.X] = label1;
                                }
                                regions.Remove(label2);
                                brightnessDictionary.Remove(label2);
                            }
                        }
                        else if (col > 0 && Math.Abs(brightness - brightnessLeft) < threshold)
                        {
                            int label = labels[row][col - 1];
                            labels[row][col] = label;
                            regions[label].Add(new Point(col, row));
                            brightnessDictionary[label] += brightness;
                        }
                        else if (row > 0 && Math.Abs(brightness - brightnessTop) < threshold)
                        {
                            int label = labels[row - 1][col];
                            labels[row][col] = label;
                            regions[label].Add(new Point(col, row));
                            brightnessDictionary[label] += brightness;
                        }
                        else
                        {
                            labelNumber++;
                            regions[labelNumber] = new List<Point>
                            {
                                new Point(col, row)
                            };
                            labels[row][col] = labelNumber;
                            brightnessDictionary[labelNumber] = brightness;
                        }
                    }
                }

                foreach (var points in regions)
                {
                    var r = (byte)Random.Next(255);
                    var g = (byte)Random.Next(255);
                    var b = (byte)Random.Next(255);
                    foreach (var point in points.Value)
                    {
                        int index = point.Y * Stride + point.X * 3;
                        buffer[index + 2] = r;
                        buffer[index + 1] = g;
                        buffer[index] = b;
                    }
                }
            });
        }


        public Bitmap SegmentationAdapitiveThreshold()
        {
            _function = "Regions Adaptive Threshold";
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                int[] histogram = new int[256];
                int max = 0;
                byte[] colors = new byte[768];
                for (int row = 0; row < bitmapData.Height; row++)
                {
                    int byteIndex = row * bitmapData.Stride;
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = byteIndex + col * 3;
                        int y = GetInt(ClassColorModel.GetBrightness(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex]));
                        histogram[y]++;
                        if (histogram[y] > max) max = histogram[y];
                    }
                }

                /*
                int min = histogram[0];
                int lastIndex = 0;
                for(int i=10;i<histogram.Length;i++)
                {
                    if (histogram[i] < min + 10)
                    {
                        min = histogram[i]-10;
                    }
                    else
                    {
                        byte r = (byte)Random.Next(256);
                        byte g = (byte)Random.Next(256);
                        byte b = (byte)Random.Next(256);
                        for (int j = lastIndex; j < i; j++)
                        {
                            colors[j + 2] = r;
                            colors[j + 1] = g;
                            colors[j] = b;
                        }
                        lastIndex = i;
                        i += 10;
                    }
                }
                */

                byte r = (byte)Random.Next(256);
                byte g = (byte)Random.Next(256);
                byte b = (byte)Random.Next(256);
                for (int j = 0; j < 15; j++)
                {
                    colors[j * 3 + 2] = r;
                    colors[j * 3 + 1] = g;
                    colors[j * 3] = b;
                }

                r = (byte)Random.Next(256);
                g = (byte)Random.Next(256);
                b = (byte)Random.Next(256);
                for (int j = 15; j < 50; j++)
                {
                    colors[j * 3 + 2] = r;
                    colors[j * 3 + 1] = g;
                    colors[j * 3] = b;
                }

                r = (byte)Random.Next(256);
                g = (byte)Random.Next(256);
                b = (byte)Random.Next(256);
                for (int j = 50; j < 120; j++)
                {
                    colors[j * 3 + 2] = r;
                    colors[j * 3 + 1] = g;
                    colors[j * 3] = b;
                }

                r = (byte)Random.Next(256);
                g = (byte)Random.Next(256);
                b = (byte)Random.Next(256);
                for (int j = 120; j < 256; j++)
                {
                    colors[j * 3 + 2] = r;
                    colors[j * 3 + 1] = g;
                    colors[j * 3] = b;
                }

                for (int row = 0; row < bitmapData.Height; row++)
                {
                    for (int col = 0; col < bitmapData.Width; col++)
                    {
                        int pixelIndex = row * Stride + col * 3;
                        var y = GetInt(ClassColorModel.GetBrightness(buffer[pixelIndex + 2], buffer[pixelIndex + 1], buffer[pixelIndex])) * 3;
                        buffer[pixelIndex + 2] = colors[y + 2];
                        buffer[pixelIndex + 1] = colors[y + 1];
                        buffer[pixelIndex] = colors[y];
                    }
                }
            });
        }

        #endregion

        #region QualityMeasurement

        public static double GetMse(byte[] filteredBuffer)
        {
            double mark = filteredBuffer.Select((t, i) => Math.Pow(_primaryImage[i] - t, 2)).Sum();
            return mark / filteredBuffer.Length;
        }

        public static double GetMsad(byte[] filteredBuffer)
        {
            double mark = filteredBuffer.Select((t, i) => Math.Abs(_primaryImage[i] - t)).Sum();
            return mark / filteredBuffer.Length;
        }

        public static double GetPsnr(double mse)
        {
            return mse == 0 ? 0 : 10f * Math.Log10(65536) / mse;
        }

        #endregion

        #region Help methods
        public static Bitmap ReadBytes(ImageLockMode imageLockMode, Action<byte[], BitmapData> action)
        {
            return ReadBytes(_image, imageLockMode, action);
        }

        protected static Bitmap ReadBytes(Image image, ImageLockMode imageLockMode, Action<byte[], BitmapData> action)
        {
            bool readWrite = imageLockMode == ImageLockMode.ReadWrite;
            Bitmap bitmap = (Bitmap)(readWrite ? image.Clone() : image);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            //Блокируем байты изображения на время манупуляций
            BitmapData bitmapData = bitmap.LockBits(rect, imageLockMode, PixelFormat.Format24bppRgb);
            //Указатель на пиксельное содержимое изображения
            IntPtr ptr = bitmapData.Scan0;
            Stride = bitmapData.Stride;
            int length = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[length];
            //Копируем данные изображения в байтовый массив
            Marshal.Copy(ptr, buffer, 0, length);
            Stopwatch time = Stopwatch.StartNew();
            time.Start();
            int count = GetRunTestCount();
            if (count == 1)
            {
                action(buffer, bitmapData);
            }
            else
            {
                byte[] bufferCopy = null;
                for (int i = 0; i < count; i++)
                {
                    bufferCopy = buffer.ToArray();
                    action(bufferCopy, bitmapData);
                }
                buffer = bufferCopy;
            }
            time.Stop();
            _time = time.Elapsed.TotalSeconds / count;
            if (IsCallbackSet)
            {
                var mse = GetMse(buffer);
                var msad = GetMsad(buffer);
                var psnr = GetPsnr(mse);
                var metricsInfo = new MetricsInfo
                {
                    Mse = mse,
                    Msad = msad,
                    Psnr = psnr
                };
                _imageProcessResult(LastWorkItem, metricsInfo);
            }

            //Копируем обратно
            if (readWrite) Marshal.Copy(buffer, 0, ptr, length);
            //Разблокируем байты изображения
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        private int GetInt(double value)
        {
            return (int)Math.Round(value);
        }

        public static int GetRunTestCount()
        {
            return string.IsNullOrEmpty(_function) ? 1 : _runTestCount;
        }

        private static bool IsCallbackSet
        {
            get
            {
                return _imageProcessResult != null && !string.IsNullOrEmpty(_function);
            }
        }

        private byte GetByte(double value)
        {
            return (byte)Math.Round(value);
        }

        double GetInvertColor(byte color)
        {
            return color == 0 ? 0 : 1d / color;
        }

        byte GetInvertColor(double color, int baseValue)
        {
            return (byte)ClassColorModel.GetValue(color < 0.03 ? 0 : baseValue / color, 0, 255);
        }

        int GetFilterSize(int radius)
        {
            return (radius * 2 + 1) * (radius * 2 + 1);
        }

        int GetRandomPixel()
        {
            return Random.Next(Height) * Stride + Random.Next(Width) * 3;
        }

        public void SetImageProcessResult(Action<WorkItem, MetricsInfo> imageProcessResult)
        {
            _imageProcessResult = imageProcessResult;
        }

        #region Kernels
        public double[] GetGausKernel(int radius, double sigma)
        {
            var kernelSize = GetFilterSize(radius);
            var sum = 0d;
            int k = 0;
            double kk = 1d / (2 * Math.PI * sigma * sigma);
            var gaussKernel = new double[kernelSize];
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    var val = kk * Math.Exp(-(i * i + j * j) / (2 * sigma * sigma));
                    gaussKernel[k] = val;
                    sum += val;
                    k++;
                }
            }
            for (int i = 0; i < gaussKernel.Length; i++) gaussKernel[i] /= sum;
            return gaussKernel;
        }

        private bool[] GetMorphologizationKernel(int structureElement, int radius)
        {
            if (structureElement == 0) return GetSqureStructureElement(radius);
            if (structureElement == 1) return GetCircleStructureElement(radius);
            return GetCrossStructureElement(radius);
        }


        private bool[] GetSqureStructureElement(int radius)
        {
            var kernelSize = 2 * radius + 1;
            var kernel = new bool[kernelSize * kernelSize];
            for (var i = 0; i < kernel.Length; i++)
            {
                kernel[i] = true;
            }
            return kernel;
        }

        private bool[] GetCrossStructureElement(int radius)
        {
            var kernelSize = 2 * radius + 1;
            int k = 0;
            var kernel = new bool[kernelSize * kernelSize];
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[k] = i == 0 || j == 0;
                    k++;
                }
            }
            return kernel;
        }

        private bool[] GetCircleStructureElement(int radius)
        {
            var kernelSize = 2 * radius + 1;
            int k = 0;
            var kernel = new bool[kernelSize * kernelSize];
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[k] = Math.Sqrt(i * i + j * j) <= radius;
                    k++;
                }
            }
            return kernel;
        }
        #endregion

        #endregion

        #region Harris Points

        int[][] ah = { new[] { 1, 0, -1 }, new[] { 1, 0, -1 }, new[] { 1, 0, -1 } };
        int[][] av = { new[] { 1, 1, 1 }, new[] { 0, 0, 0 }, new[] { -1, -1, -1 } };

        private double[][] CreateImageMatrix()
        {
            double[][] matrix = new double[Height][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new double[Width];
            }
            return matrix;
        }

        public List<HarrisPoint> FindHarrisPoints(int threshold, int rang)
        {
            List<HarrisPoint> points = new List<HarrisPoint>();

            var grayImage = CreateImageMatrix();
            var diffX = CreateImageMatrix();
            var diffY = CreateImageMatrix();
            var diffXy = CreateImageMatrix();


            //Creating brightness matrix
            for (int row = 0; row < Height; row++)
            {
                int byteIndex = row * Stride;
                for (int col = 0; col < Width; col++)
                {
                    int pixelIndex = byteIndex + col * 3;
                    //Формирование массива яркости
                    grayImage[row][col] = ClassColorModel.GetBrightness(_original[pixelIndex + 2], _original[pixelIndex + 1], _original[pixelIndex]);
                }
            }

            //var gaus = FilterGaus(gausR, gausSigma);

            //Calculating gradient
            int width = Width - 1;
            int height = Height - 1;
            double[][] tx = CreateImageMatrix();
            double[][] ty = CreateImageMatrix();
            double g1, g2;
            for (int i = 1; i < height; i++)
            {
                for (int j = 1; j < width; j++)
                {
                    g1 = 0;
                    g2 = 0;
                    for (int k = 0; k < 3; k++)
                        for (int l = 0; l < 3; l++)
                        {
                            g1 += ah[k][l] * grayImage[i + k - 1][j + l - 1];
                            g2 += av[k][l] * grayImage[i + k - 1][j + l - 1];
                        }
                    diffX[i][j] = g1;
                    diffY[i][j] = g2;
                    diffXy[i][j] = g1;
                }
            }
            for (int i = 1; i < height; i++)
            {
                for (int j = 1; j < width; j++)
                {
                    g1 = 0;
                    g2 = 0;
                    double g3 = 0;
                    for (int k = 0; k < 3; k++)
                        for (int l = 0; l < 3; l++)
                        {
                            g1 += ah[k][l] * diffX[i + k - 1][j + l - 1];
                            g2 += av[k][l] * diffY[i + k - 1][j + l - 1];
                            g3 += av[k][l] * diffX[i + k - 1][j + l - 1];
                        }
                    tx[i][j] = ClassColorModel.GetValue(g1);
                    ty[i][j] = ClassColorModel.GetValue(g2);
                    diffXy[i][j] = ClassColorModel.GetValue(g3);
                }
            }
            diffX = tx;
            diffY = ty;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    //Функция отклика угла
                    double har = diffX[i][j] * diffY[i][j] - Math.Pow(diffXy[i][j], 2) - 0.04 * Math.Pow(diffX[i][j] + diffY[i][j], 2);
                    if (har > threshold)
                    {
                        points.Add(new HarrisPoint(j, i, har));
                    }
                }
            }

            points.Sort();

            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                for (var j = i + 1; j < points.Count; j++)
                {
                    if (point.Dist(points[j]) < rang)
                    {
                        points.RemoveAt(j);
                        j--;
                    }
                }
            }

            return points;
        }

        public Bitmap HighlightPoints(List<HarrisPoint> points)
        {
            return ReadBytes(ImageLockMode.ReadWrite, (buffer, bitmapData) =>
            {
                foreach (var point in points)
                {
                    int pixelIndex = point.Y * bitmapData.Stride + point.X * 3;
                    buffer[pixelIndex + 2] = 255;
                    buffer[pixelIndex + 1] = 0;
                    buffer[pixelIndex] = 0;
                }
            });
        }

        

        #endregion
    }
}

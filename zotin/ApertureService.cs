using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zotin
{
    class ApertureService
    {

        public void getAperturePosition(int x, int y, Point imageSize, int i, int j, Point filterSize, out int pixelPositionX, out int pixelPositionY)
        {
            int centerFilterX = (int)(filterSize.X / 2);
            int centerFilterY = (int)(filterSize.Y / 2);

            pixelPositionX = x + j - centerFilterX;
            pixelPositionY = y + i - centerFilterY;

            if (pixelPositionX < 0)
                pixelPositionX += centerFilterX;

            if (pixelPositionY < 0)
                pixelPositionY += centerFilterY;

            if (pixelPositionX >= imageSize.X)
                pixelPositionX -= centerFilterX;

            if (pixelPositionY >= imageSize.Y)
                pixelPositionY -= centerFilterY;

            if (pixelPositionX >= imageSize.X || pixelPositionY >= imageSize.Y)
            {
                pixelPositionX = imageSize.X - centerFilterX;
                pixelPositionY = imageSize.Y - centerFilterY;
            }
        }

        public List<Aperture> getApertureMatrixGenerator(Point imageSize, Point filterSize)
        {
            List<Aperture> apertures = new List<Aperture>();
            for (int x = 0; x < imageSize.X; x++)
            {
                for (int y = 0; y < imageSize.Y; y++)
                {
                    int pixelPosX = 0, pixelPosY = 0;
                    List<List<Point>> apertureMatrix = new List<List<Point>>();

                    for (int i = 0; i < filterSize.X; i++)
                    {
                        List<Point> apertureLine = new List<Point>();
                        for (int j = 0; j < filterSize.Y; j++)
                        {
                            getAperturePosition(x, y, imageSize, i, j, filterSize, out pixelPosX, out pixelPosY);

                            if (pixelPosX == -1 || pixelPosY == -1)
                                continue;

                            apertureLine.Add(new Point(pixelPosX, pixelPosY));
                        }
                        apertureMatrix.Add(apertureLine);
                    }
                    apertures.Add(new Aperture(x, y, apertureMatrix));
                }
            }
            return apertures;
        }

    }

    public class Aperture : IDisposable
    {
        public int x, y;
        public List<List<Point>> matrix;

        public Aperture(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public Aperture(int X, int Y, List<List<Point>> Matrix)
        {
            x = X;
            y = Y;
            matrix = Matrix;
        }

        public void Dispose()
        {
            if (matrix != null)
            {
                for (int i = 0; i < matrix.Count; i++)
                {
                    matrix[i].Clear();
                }
                matrix.Clear();
            }
        }
    }
}

namespace zotin
{
    class PixelBufferHelper
    {
        public static void SetPixel(byte[] buffer, int index, int color)
        {
            if (color < 0) buffer[index] = 0; else if (color > 255) buffer[index] = 255; else buffer[index] = (byte)color;
        }
    }
}

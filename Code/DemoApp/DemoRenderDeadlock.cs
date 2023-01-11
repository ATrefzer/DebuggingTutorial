using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DemoApp
{
    internal class DemoRenderDeadlock
    {
        public ImageSource CreateBitMapNoLocking()
        {
            const int height = 100;
            const int width = 100;
            var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            var buffer = new byte[bitmap.BackBufferStride * height];

            var random = new Random();
            for (var row = 0; row < 100; row++)
            for (var column = 0; column < 100; column++)
            {
                var red = (byte) random.Next(byte.MaxValue);
                var green = (byte) random.Next(byte.MaxValue);
                var blue = (byte) random.Next(byte.MaxValue);
                var alpha = (byte) random.Next(byte.MaxValue);

                buffer[row * bitmap.BackBufferStride + column * 4] = blue;
                buffer[row * bitmap.BackBufferStride + column * 4 + 1] = green;
                buffer[row * bitmap.BackBufferStride + column * 4 + 2] = red;
                buffer[row * bitmap.BackBufferStride + column * 4 + 3] = alpha;
            }

            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight),
                buffer, bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8, 0);
            return bitmap;
        }

        private int GetRandomColorArgb()
        {
            var random = new Random();

            var red = (byte) random.Next(byte.MaxValue);
            var green = (byte) random.Next(byte.MaxValue);
            var blue = (byte) random.Next(byte.MaxValue);
            var alpha = (byte) random.Next(byte.MaxValue);

            var argb = 0;

            argb |= alpha << 24;
            argb |= red << 16;
            argb |= green << 8;
            argb |= blue; // Lsb
            return argb;
        }

        public ImageSource CreateBitMapDirectBackbufferWrite()
        {
            var height = 100;
            var width = 100;
            var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            bitmap.Lock();

            unsafe
            {
                for (var row = 0; row < 100; row++)
                {
                    for (var column = 0; column < 100; column++) // Each pixel is represented as integer
                    {
                        var argb = GetRandomColorArgb();


                        // Get a pointer to the back buffer.
                        var pBackBuffer = bitmap.BackBuffer;

                        // Find the address of the pixel to draw.
                        //var pixelPos = row * bitmap.BackBufferStride + column * 4; // Byte position
                        var pixelPos = row * bitmap.BackBufferStride / sizeof(int) + column;

                        // Assign the color data to the pixel.
                        *((int*) pBackBuffer + pixelPos) = argb;
                    }
                }
            }

            // Specify the area of the bitmap that changed.
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));

            // TODO DEADLOCK
            // bitmap.Unlock();

            return bitmap;
        }
    }
}
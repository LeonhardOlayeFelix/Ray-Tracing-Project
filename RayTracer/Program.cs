using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RayTracer
{
    public static class Program
    {
        public static WriteableBitmap ProduceBitmap(int[,,] source)
        {
            int height = source.GetLength(0);
            int width = source.GetLength(1);
            WriteableBitmap writeableBitmap = new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            writeableBitmap.Lock();

            unsafe
            {
                byte* basePtr = (byte*)writeableBitmap.BackBuffer;
                int stride = writeableBitmap.BackBufferStride;

                for (int y = 0; y < height; y++)
                {
                    byte* rowPtr = basePtr + y * stride;

                    for (int x = 0; x < width; x++)
                    {
                        int r = source[y, x, 0];
                        int g = source[y, x, 1];
                        int b = source[y, x, 2];

                        rowPtr[x * 4 + 0] = (byte)b;
                        rowPtr[x * 4 + 1] = (byte)g;
                        rowPtr[x * 4 + 2] = (byte)r;
                        rowPtr[x * 4 + 3] = 255;
                    }
                }
            }

            writeableBitmap.AddDirtyRect(
                new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight)
            );

            writeableBitmap.Unlock();

            return writeableBitmap;

        }
        public static object Main(IProgress<double> progress)
        {
            int image_width = 256;
            int image_height = 256;
            int color_depth = 3;
            int[,,] bitmap = new int[image_height, image_width, color_depth];

            int total = image_height * image_width;
            int count = 0;

            for (int i = 0; i < image_height; i++)
            {
                for (int j =0; j < image_width; j++)
                {
                    double r = (double)i / (image_height - 1);
                    double g = (double)j / (image_height - 1);
                    double b = 0.0;

                    bitmap[i, j, 0] = (int)(255.9999 * r);
                    bitmap[i, j, 1] = (int)(255.9999 * g);
                    bitmap[i, j, 2] = (int)(255.9999 * b);

                    count++;

                    if (count % 500 == 0)
                        progress?.Report((double)count / total);
                }
            }

            return ProduceBitmap(bitmap);
        }
    }
}

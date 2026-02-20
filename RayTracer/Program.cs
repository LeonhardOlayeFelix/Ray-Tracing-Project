using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = RayTracer.Vec3;
using Colour = RayTracer.Vec3;
using ProjectUtilities;

namespace RayTracer
{
    public static class Program
    {

        public static object Main(IProgress<double> progress)
        {
            Logger.Initialise("C:\\Users\\leonh\\OneDrive\\Desktop\\log");

            //setup
            double aspectRatio = 16.0 / 9.0;
            int imageWidth = 400;
            int imageHeight = (int)(imageWidth / aspectRatio);
            double viewportHeight = 2.0;
            double viewportWidth = viewportHeight * ((double)imageWidth / imageHeight);

            Logger.Log("Hello");

            imageHeight = imageHeight < 1 ? 1 : imageHeight;
            int color_depth = 3;
            int[,,] bitmap = new int[imageHeight, imageWidth, color_depth];
            int total = imageHeight * imageWidth;
            int count = 0;



            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    double r = (double)i / (imageHeight - 1);
                    double g = (double)j / (imageWidth - 1);
                    double b = 0.0;

                    WriteColour(i, j, new Colour(r, g, b), bitmap);

                    count++;

                    if (count % 500 == 0)
                        progress?.Report((double)count / total);
                }
            }

            return ProduceBitmap(bitmap);
        }
        private static WriteableBitmap ProduceBitmap(int[,,] source)
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

        private static void WriteColour(int i, int j, Colour colour, int[,,] bitmap)
        {
            bitmap[i, j, 0] = (int)(255.9999 * colour[0]);
            bitmap[i, j, 1] = (int)(255.9999 * colour[1]);
            bitmap[i, j, 2] = (int)(255.9999 * colour[2]);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
using Point = RayTracer.Vec3;

namespace RayTracer
{
    public class Camera
    {
        private int _imageHeight;
        private Point _center;
        private Point P_00;
        private Direction Du;
        private Direction Dv;
        private int[,,] bitmap;
        private int color_depth = 3;
        public double AspectRatio = 1.0;
        public int ImageWidth = 100;
        public int SamplingRate = 1;
        public WriteableBitmap render(Hittable world)
        {
            Initialise();

            for (int i = 0; i < _imageHeight; i++)
            {
                for (int j = 0; j < ImageWidth; j++)
                {
                    Colour pixelColour = new Colour(0, 0, 0);
                    Point pixelLocation = P_00 + j * Du + i * Dv;
                    for (int k = 0; k < SamplingRate; k++)
                    {
                        Point HitLocation = pixelLocation + 0.5 * MathHelper.Random() * Dv + 0.5 * MathHelper.Random() * Du;
                        Direction rayDirection = HitLocation - _center;
                        Ray ray = new Ray(_center, rayDirection);

                        pixelColour += RayColour(ray, world);
                    }

                    pixelColour = pixelColour / SamplingRate;
                    WriteColour(i, j, pixelColour, bitmap);
                }
            }

            return ProduceBitmap(bitmap);
        }

        private void Initialise()
        {

            _imageHeight = (int)(ImageWidth / AspectRatio);
            _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;

            bitmap = new int[_imageHeight, ImageWidth, color_depth];

            _center = new Point(0, 0, 0);
            double focalLength = 1.0;
            double viewportHeight = 2.0;
            double viewportWidth = viewportHeight * ((double)ImageWidth / _imageHeight);

            Direction Vu = new Direction(viewportWidth, 0, 0);
            Direction Vv = new Direction(0, -viewportHeight, 0);
            Du = Vu / ImageWidth;
            Dv = Vv / _imageHeight;
            Point Q = _center - new Direction(0, 0, focalLength) - 0.5 * (Vv + Vu);
            P_00 = Q + 0.5 * (Du + Dv);
        }

        private Colour RayColour(Ray ray, Hittable world)
        {
            HitInfo hitInfo = new HitInfo();

            if (world.hit(ray, new Interval(0, MathHelper.Infinity), ref hitInfo))
            {
                return 0.5 * new Colour(hitInfo.Normal.X + 1, hitInfo.Normal.Y + 1, hitInfo.Normal.Z + 1);
            }

            Vec3 unitDirection = ray.Direction.UnitVector;
            double a = 0.5 * (unitDirection.Y + 1.0);
            return (1.0 - a) * new Colour(1.0, 1.0, 1.0) + a * new Colour(0.5, 0.7, 1.0);
        }

        private static void WriteColour(int row, int col, Colour colour, int[,,] bitmap)
        {
            bitmap[row, col, 0] = (int)(255.9999 * colour[0]);
            bitmap[row, col, 1] = (int)(255.9999 * colour[1]);
            bitmap[row, col, 2] = (int)(255.9999 * colour[2]);
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
    }
}

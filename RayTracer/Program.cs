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
using Direction = RayTracer.Vec3;
using ProjectUtilities;

namespace RayTracer
{
    public static class Program
    {
        public static Point Center;
        public static double Radius;
        public static object Main(Point center, double radius)
        {
            Center = center;
            Radius = radius;
            //Logger.Initialise("C:\\Users\\leonh\\OneDrive\\Desktop\\log");

            //Image
            double aspectRatio = 16.0 / 9.0;
            int imageWidth = 400;
            int imageHeight = (int)(imageWidth / aspectRatio);

            //Camera
            double focalLength = 1.0;
            double viewportHeight = 2.0;
            double viewportWidth = viewportHeight * ((double)imageWidth / imageHeight);

            //Vector setup
            Point C = new Point(0, 0, 0);
            Direction Vu = new Direction(viewportWidth, 0, 0);
            Direction Vv = new Direction(0, -viewportHeight, 0);
            Direction Du = Vu / imageWidth;
            Direction Dv = Vv / imageHeight;
            Point Q = C - new Direction(0, 0, focalLength) - 0.5 * (Vv + Vu);
            Point P_00 = Q + 0.5 * (Du + Dv);

            //#region Logging
            //Logger.Log("===================================Setup=========================================");
            //Logger.Log("Image Setup:");
            //Logger.Log($"   Aspect Ratio: {aspectRatio}");
            //Logger.Log($"   Pixel Width: {imageWidth}");
            //Logger.Log($"   Pixel Height: {imageHeight}");
            //Logger.Log("Camera Setup:");
            //Logger.Log($"   Focal Length: {focalLength}");
            //Logger.Log($"   Camera _center: {C}");
            //Logger.Log("Vector Setup:");
            //Logger.Log($"   Viewport Width: {viewportWidth}");
            //Logger.Log($"   Viewport Height: {viewportHeight}");
            //Logger.Log($"   Vu: {Vu}");
            //Logger.Log($"   Vv: {Vv}");
            //Logger.Log($"   Du: {Du}");
            //Logger.Log($"   Dv: {Dv}");
            //Logger.Log($"   Q: {Q}");
            //Logger.Log($"   P_00: {P_00}");
            //#endregion

            imageHeight = imageHeight < 1 ? 1 : imageHeight;
            int color_depth = 3;
            int[,,] bitmap = new int[imageHeight, imageWidth, color_depth];
            int total = imageHeight * imageWidth;
            int count = 0;



            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    Point pixelLocation = P_00 + j * Du + i * Dv;
                    Direction rayDirection = pixelLocation - C;
                    Ray ray = new Ray(C, rayDirection);

                    Colour pixelColour = ComputeRayColour(ray);
                    WriteColour(i, j, pixelColour, bitmap);

                    count++;
                }
            }

            return ProduceBitmap(bitmap);
        }

        private static Colour ComputeRayColour(Ray ray)
        {
            Sphere sphere = new Sphere(Center, Radius);
            HitInfo hitInfo = new HitInfo();
            sphere.hit(ray, 0, 10000, hitInfo);

            if (hitInfo.t > 0.0)
            {
                return 0.5 * new Colour(hitInfo.Normal.X + 1, hitInfo.Normal.Y + 1, hitInfo.Normal.Z + 1);
            }

            Vec3 unitDirection = ray.Direction.UnitVector;
            double a = 0.5 * (unitDirection.Y + 1.0);
            return (1.0 - a) * new Colour(1.0, 1.0, 1.0) + a * new Colour(0.5, 0.7, 1.0);
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

        private static void WriteColour(int row, int col, Colour colour, int[,,] bitmap)
        {
            bitmap[row, col, 0] = (int)(255.9999 * colour[0]);
            bitmap[row, col, 1] = (int)(255.9999 * colour[1]);
            bitmap[row, col, 2] = (int)(255.9999 * colour[2]);
        }

        private static double HitSphere(Point center, double radius, Ray ray)
        {
            center = Center;
            radius = Radius;
            Direction oc = center - ray.Origin;
            double a = ray.Direction.LengthSquared;
            double h = Vec3Util.Dot(ray.Direction, oc);
            double c = oc.LengthSquared - radius * radius;
            double discriminant = h*h - a*c;

            if (discriminant < 0)
                return -1.0;
            return (h - Math.Sqrt(discriminant)) / a;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
using Point = RayTracer.Vec3;
using Displacement = RayTracer.Vec3;

namespace RayTracer
{

    public class Camera
    {
        private static Interval _intensity = new Interval(0, 0.999999);
        private int _imageHeight;
        private Point _center;
        private Point P_00;
        private Direction Du;
        private Direction Dv;
        private int[,,] bitmap;
        private int color_depth = 3;
        private int _samplesPerPixel = 200;
        public double AspectRatio = 1.0;
        public int ImageWidth = 456;
        public int[,,] render(Hittable world)
        {
            Initialise();
            int count = 0;
            int total = ImageWidth * _imageHeight;

            for (int i = 0; i < _imageHeight; i++)
            {
                for (int j = 0; j < ImageWidth; j++)
                {
                    Point pixelLocation = P_00 + i * Dv + j * Du;
                    Colour pixelColour = new Colour(0, 0, 0);
                    for (int k = 0; k < _samplesPerPixel; k++)
                    {
                        Ray ray = GetOffsetRay(i, j);
                        pixelColour += RayColour(ray, world);
                    }
                    count++;
                    Program.Progress.RenderProgress.Report((double)count / total);
                    WriteColour(i, j, pixelColour / _samplesPerPixel, bitmap);
                }
            }

            return bitmap;
        }

        private Ray GetOffsetRay(int i, int j)
        {
            Interval interval = new Interval(-0.5, 0.5);
            Displacement Offset = MathHelper.RandomVec3(interval, interval, null);

            Point q_n = P_00 + Dv * (i + Offset.X) + Du * (j + Offset.Y);
            Point C = _center;

            return new Ray(C, q_n - C);
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
            bitmap[row, col, 0] = (int)(256 * _intensity.Clamp(colour[0]));
            bitmap[row, col, 1] = (int)(256 * _intensity.Clamp(colour[1]));
            bitmap[row, col, 2] = (int)(256 * _intensity.Clamp(colour[2]));
        }

        
    }
}

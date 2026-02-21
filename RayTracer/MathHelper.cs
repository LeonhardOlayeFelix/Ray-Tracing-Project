using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public static class MathHelper
    {
        private static readonly Random _gen = new Random();
        public static double Infinity => int.MaxValue;
        public static double Pi => 3.1415926535897932385;
        public static double DegreesToRadian(double degrees)
        {
            return degrees * Pi / 180.0;
        }
        public static double Random()
        {
            return _gen.Next(0, 1);
        }
        public static double RandomMinMax(double min, double max)
        {
            return min + (max - min) * Random();
        }
    }
}

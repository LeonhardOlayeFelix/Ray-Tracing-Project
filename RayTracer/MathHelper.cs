using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static double RandomDouble()
        {
            return _gen.NextDouble();
        }
        public static double RandomDouble(double min, double max)
        {
            return min + (max - min) * RandomDouble();
        }
        public static double RandomDouble(Interval interval)
        {
            return RandomDouble(interval.Min, interval.Max);
        }
        public static double LinearToGamma(double val)
        {
            if (val > 0)
            {
                return Math.Sqrt(val);
            }
            return 0;
        }
    }
}

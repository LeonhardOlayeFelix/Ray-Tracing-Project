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
        public static Vec3 RandomVec3(Interval? xInterval, Interval? yInterval, Interval? zInterval)
        {
            xInterval ??= new Interval(0, 0);
            yInterval ??= new Interval(0, 0);
            zInterval ??= new Interval(0, 0);
            return new Vec3(RandomDouble(xInterval), RandomDouble(yInterval), RandomDouble(zInterval));
        }
        public static Vec3 RandomVec3(Interval interval)
        {
            return RandomVec3(interval, interval, interval);
        }
    }
}

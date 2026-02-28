using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RayTracer
{
    public class Vec3
    {
        double[] e;
        public double X => e[0];
        public double Y => e[1];
        public double Z => e[2];
        public double this[int i]
        {
            get => e[i];
            set => e[i] = value;
        }
        public Vec3()
        {
            e = [0, 0, 0];
        }
        public Vec3(double e0, double e1, double e2)
        {
            e = [e0, e1, e2];
        }
        public static Vec3 operator -(Vec3 v) => new Vec3(-v.e[0], -v.e[1], -v.e[2]);
        public static Vec3 operator +(Vec3 u, Vec3 v) => new Vec3(u.e[0] + v.e[0], u.e[1] + v.e[1], u.e[2] + v.e[2]);
        public static Vec3 operator -(Vec3 u, Vec3 v) => new Vec3(u.e[0] - v.e[0], u.e[1] - v.e[1], u.e[2] - v.e[2]);
        public static Vec3 operator *(Vec3 u, Vec3 v) => new Vec3(u.e[0] * v.e[0], u.e[1] * v.e[1], u.e[2] * v.e[2]);
        public static Vec3 operator *(double t, Vec3 v) => new Vec3(t * v.e[0], t * v.e[1], t * v.e[2]);
        public static Vec3 operator *(Vec3 v, double t) => t * v;
        public static Vec3 operator /(Vec3 v, double t) => (1.0 / t) * v;
        public double Length => Math.Sqrt(LengthSquared);
        public double LengthSquared => e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
        public Vec3 UnitVector => Vec3Util.UnitVector(this);
        public Vec3 AddInPlace(Vec3 v)
        {
            e[0] += v.e[0];
            e[1] += v.e[1];
            e[2] += v.e[2];
            return this;
        }
        public Vec3 MulInPlace(double t)
        {
            e[0] *= t;
            e[1] *= t;
            e[2] *= t;
            return this;
        }
        public Vec3 DivInPlace(double t) => MulInPlace(1.0 / t);
        public override string ToString() => $"{e[0]} {e[1]} {e[2]}";
    }
    public static class Vec3Util
    {
        public static double Dot(Vec3 u, Vec3 v) => u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        public static Vec3 Cross(Vec3 u, Vec3 v) => new Vec3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X);
        public static Vec3 UnitVector(Vec3 v) => v / v.Length; 
        public static Vec3 RandomVec3(Interval? xInterval, Interval? yInterval, Interval? zInterval)
        {
            xInterval ??= new Interval(0, 0);
            yInterval ??= new Interval(0, 0);
            zInterval ??= new Interval(0, 0);
            return new Vec3(MathHelper.RandomDouble(xInterval), MathHelper.RandomDouble(yInterval), MathHelper.RandomDouble(zInterval));
        }
        public static Vec3 RandomVec3()
        {
            return new Vec3(MathHelper.RandomDouble(), MathHelper.RandomDouble(), MathHelper.RandomDouble());
        }
        public static Vec3 RandomVec3(Interval interval)
        {
            return RandomVec3(interval, interval, interval);
        }
        public static Vec3 SampleXYSquare(double length)
        {
            Interval interval = new Interval(-length, length);
            return RandomVec3(interval, interval, null);
        }
        private static Vec3 SampleCube(double length)
        {
            Interval interval = new Interval(-1, 1);
            return RandomVec3(interval);
        }
        public static Vec3 UniformUnitSphere()
        {
            while (true)
            {
                Vec3 random = SampleCube(1);
                double lengthSquared = random.LengthSquared;
                if (1e-160 <= lengthSquared && lengthSquared <= 1)
                    return random / lengthSquared;
            }

        }
        public static Vec3 UniformUnitHemisphere(Vec3 normal)
        {
            Vec3 result = UniformUnitSphere();
            if (Dot(normal, result) > 0.0)
                return result;
            return -result;
        }
    } 
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = RayTracer.Vec3;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
using System.Transactions;

namespace RayTracer
{
    public abstract class Hittable
    {
        public abstract bool hit(Ray ray, double t_min, double t_max, HitInfo hitInfo);
    }
    public class Sphere : Hittable
    {
        private Point _center { get; set; }
        private double _radius { get; set; }
        public Sphere(Point center, double radius)
        {
            _center = center;
            _radius = radius;
        }
        public override bool hit(Ray ray, double t_min, double t_max, HitInfo hitInfo)
        {
            Direction oc = _center - ray.Origin;
            double a = ray.Direction.LengthSquared;
            double h = Vec3Util.Dot(ray.Direction, oc);
            double c = oc.LengthSquared - _radius * _radius;
            double discriminant = h * h - a * c;

            if (discriminant < 0)
                return false;

            double sqrt = Math.Sqrt(discriminant);

            double t = (h - sqrt) / a;

            if (t <= t_min || t >= t_max)
            {
                t = (h + sqrt) / a;
                if (t <= t_min || t >= t_max)
                    return false;
            }

            hitInfo.IntersectionPoint = ray.At(t);
            hitInfo.t = t;
            hitInfo.Normal = (hitInfo.IntersectionPoint - _center).UnitVector;
            return true;
        }
    }
}

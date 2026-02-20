using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = RayTracer.Vec3;
using Direction = RayTracer.Vec3;

namespace RayTracer
{
    public class Ray
    {
        public Point Origin { get; set; }
        public Direction Direction { get; set; }
        public Ray(Point origin, Direction direction)
        {
            Origin = origin;
            Direction = direction;
        }
        public Point At(double t)
        {
            return Origin + t * Direction;
        }
    }
}

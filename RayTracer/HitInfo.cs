using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = RayTracer.Vec3;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;

namespace RayTracer
{
    public class HitInfo
    {
        public Point IntersectionPoint { get; set; }
        public Direction Normal { get; set; }
        public double t { get; set; }
    }
}

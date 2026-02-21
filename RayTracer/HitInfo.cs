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
        public bool FrontFace { get; set; }

        public void SetFaceNormal(Ray ray, Direction outwardsNormal)
        {
            //Always want the normal pointing in the opposite direction of the ray
            double dot = Vec3Util.Dot(ray.Direction, outwardsNormal);
            Normal = dot > 0.0 ? -outwardsNormal : outwardsNormal;
        }
    }
}

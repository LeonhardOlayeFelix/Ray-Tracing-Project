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
        public abstract bool hit(Ray ray, Interval bounds, ref HitInfo hitInfo);
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class HittableCollection : Hittable
    {
        List<Hittable> objects;

        public HittableCollection()
        {
            objects = new List<Hittable>();
        }
        public void Add(Hittable objectToAdd)
        {
            objects.Add(objectToAdd);
        }
        public void Clear()
        {
            objects.Clear();
        }
        public override bool hit(Ray ray, Interval bounds, ref HitInfo hitInfo)
        {
            HitInfo temp = new HitInfo();
            bool hit_anything = false;
            double closestSoFar = bounds.Max;

            foreach (Hittable obj in objects)
            {
                if (obj.hit(ray, new Interval(bounds.Min, closestSoFar), ref temp))
                {
                    hit_anything = true;
                    closestSoFar = temp.t;
                    hitInfo = temp;
                }
            }

            return hit_anything;
        }
    }
}

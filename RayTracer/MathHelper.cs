using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public static class MathHelper
    {
        public static double Infinity => int.MaxValue;
        public static double Pi => 3.1415926535897932385;
        public static double DegreesToRadian(double degrees)
        {
            return degrees * Pi / 180.0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
using Displacement = RayTracer.Vec3;
using Point = RayTracer.Vec3;

namespace RayTracer
{
    public abstract class Material
    {
        public abstract bool Scatter(Ray incomingRay, ref HitInfo hitInfo, ref Vec3 attenuation, ref Ray scatteredRay);
    }

    public class UpwardsMaterial : Material
    {
        public override bool Scatter(Ray incomingRay, ref HitInfo hitInfo, ref Colour attenuation, ref Ray scatteredRay)
        {
            scatteredRay = new Ray(hitInfo.IntersectionPoint, hitInfo.Normal);
            return true;
        }
    }

    public class LambertianMaterial : Material
    {
        private Colour _albedo;
        public LambertianMaterial(Colour albedo)
        {
            _albedo = albedo;
        }
        public override bool Scatter(Ray incomingRay, ref HitInfo hitInfo, ref Colour attenuation, ref Ray scatteredRay)
        {
            scatteredRay = new Ray(hitInfo.IntersectionPoint, hitInfo.Normal + Vec3Util.UniformUnitSphere());

            if (Vec3Util.NearZero(scatteredRay.Direction))
                scatteredRay.Direction = hitInfo.Normal;

            attenuation = _albedo;
            return true;
        }
    }
    public static class ColourFactory
    {
        public static Colour Red => new Colour(1, 0, 0);
        public static Colour Green => new Colour(0, 1, 0);
        public static Colour Blue => new Colour(0, 0, 1);
        public static Colour White => new Colour(1, 1, 1);
    }
    public static class MaterialFactory
    {
        public static Material RedLambertian => new LambertianMaterial(ColourFactory.Red);
        public static Material GreenLambertian => new LambertianMaterial(ColourFactory.Green);
        public static Material BlueLambertian => new LambertianMaterial(ColourFactory.Blue);
        public static Material WhiteLambertian => new LambertianMaterial(ColourFactory.White);
    }
}

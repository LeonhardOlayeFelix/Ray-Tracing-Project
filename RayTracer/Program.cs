using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = RayTracer.Vec3;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;

namespace RayTracer
{
    public static class Program
    {
        public static RayTracerProgress Progress = new RayTracerProgress();
        public static object Main()
        {
            Sphere sphere0 = new Sphere(new Point(0, -100.5, -1), 100, MaterialFactory.Ball0Material);
            Sphere sphereCenter = new Sphere(new Point(0.0, 0.0, -1.2), 0.5, MaterialFactory.Ball1Material);
            Sphere sphereLeft = new Sphere(new Point(-1.0, 0.0, -1.0), 0.5, MaterialFactory.Ball2Material);
            Sphere sphereRight = new Sphere(new Point(1.0, 0.0, -1.0), 0.5, MaterialFactory.Ball3Material);
            HittableCollection world = new HittableCollection();
            world.Add(sphere0);
            world.Add(sphereCenter);
            world.Add(sphereLeft);
            world.Add(sphereRight);

            Camera cam = new Camera();
            cam.AspectRatio = 16.0 / 9.0;
            cam.ImageWidth = 400;

            int[,,] render = cam.render(world);
            return render;
        }
    }
}

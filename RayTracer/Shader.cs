using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Shader
    {
        public Camera Camera { get; set; }

        public Shader(Camera camera)
        {
            Camera = camera;
        }
    }
}

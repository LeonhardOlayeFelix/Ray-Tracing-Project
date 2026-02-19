using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class RenderProgress : IProgress<double>
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        public void Report(double percentage)
        {
            int percent = (int)Math.Round(percentage * 100.0);
            percent = Math.Clamp(percent, 0, 100);
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(percent, null));
        }
    }
}

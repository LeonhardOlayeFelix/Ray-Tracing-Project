using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ray_Tracing_Project.Model
{
    public class MainViewModel : ObservableObject
    {
        private WriteableBitmap _source;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public MainViewModel()
        {
            Source = (WriteableBitmap)RayTracer.Program.Main();
        }
    }
}

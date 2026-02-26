using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ProjectUtilities;
using Ray_Tracing_Project.IOCContainer;
using RayTracer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = RayTracer.Vec3;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
namespace Ray_Tracing_Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ICommand _traceCommand;
        private WriteableBitmap _source;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public ICommand TraceCommand => _traceCommand ??= new RelayCommand(OnTrace);
        private void OnTrace()
        {
            Source = (WriteableBitmap)RayTracer.Program.Main();
        }
    }
}

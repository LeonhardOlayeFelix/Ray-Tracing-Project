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
        private RenderProgress _progress;
        private ICommand _traceCommand;
        private double _radius = 0.5;
        private double _x = 0;
        private double _y = 0;
        private double _z = -1;
        private WriteableBitmap _source;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public double Radius
        {
            get => _radius;
            set
            {
                SetProperty(ref _radius, value);
                OnScenePropertyChanged();
            }
        }
        public double X
        {
            get => _x;
            set
            {
                SetProperty(ref _x, value);
                OnScenePropertyChanged();
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                SetProperty(ref _y, value);
                OnScenePropertyChanged();
            }
        }
        public double Z
        {
            get => _z;
            set
            {
                SetProperty(ref _z, value);
                OnScenePropertyChanged();
            }
        }


        public double ProgressPercentage { get; set; } = 1;
        public ICommand TraceCommand => _traceCommand ??= new RelayCommand(OnTrace);
        public MainViewModel()
        {
            _progress = new RenderProgress();
            _progress.ProgressChanged += OnProgressChanged;
            
        }
        private void OnTrace()
        {
            Source = (WriteableBitmap)RayTracer.Program.Main(new Point(X, Y, Z), Radius);
        }
        private void OnProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            DispatcherHelp.CheckInvokeOnUI(() => ProgressPercentage = e.ProgressPercentage);
            OnPropertyChanged(nameof(ProgressPercentage));
        }
        private void OnScenePropertyChanged()
        {
            Source = (WriteableBitmap)RayTracer.Program.Main(new Point(X, Y, Z), Radius);
        }
    }
}

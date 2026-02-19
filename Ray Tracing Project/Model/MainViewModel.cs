using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Ray_Tracing_Project.IOCContainer;
using RayTracer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ray_Tracing_Project.Model
{
    public class MainViewModel : ObservableObject
    {
        private RenderProgress _progress;
        private WriteableBitmap _source;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public double ProgressPercentage { get; set; } = 1;
        public MainViewModel()
        {
            _progress = new RenderProgress();
            _progress.ProgressChanged += OnProgressChanged;
            Source = (WriteableBitmap)RayTracer.Program.Main(_progress);
        }

        private void OnProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressPercentage = (double)e.ProgressPercentage;
            OnPropertyChanged(nameof(ProgressPercentage));
        }
    }
}

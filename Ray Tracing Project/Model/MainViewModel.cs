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

namespace Ray_Tracing_Project.Model
{
    public class MainViewModel : ObservableObject
    {
        private RenderProgress _progress;
        private ICommand _traceCommand;
        private WriteableBitmap _source;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
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
            Source = (WriteableBitmap)RayTracer.Program.Main(_progress);
        }
        private void OnProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            DispatcherHelp.CheckInvokeOnUI(() => ProgressPercentage = (double)e.ProgressPercentage);
            OnPropertyChanged(nameof(ProgressPercentage));
        }
    }
}

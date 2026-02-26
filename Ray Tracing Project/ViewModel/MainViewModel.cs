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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Colour = RayTracer.Vec3;
using Direction = RayTracer.Vec3;
using Point = RayTracer.Vec3;
namespace Ray_Tracing_Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ICommand _traceCommand;
        private WriteableBitmap _source;
        private double _progressPercentage = 0;
        public WriteableBitmap Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public ICommand TraceCommand => _traceCommand ??= new AsyncRelayCommand(OnTrace);
        public double ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }
        public MainViewModel()
        {
            RayTracer.Program.Progress.RenderProgress.ProgressChanged += RenderProgress_ProgressChanged;
        }

        private void RenderProgress_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
        }

        private async Task OnTrace()
        {
            int[,,] bitmap = await Task.Run(() =>
            {
                return (int[,,])RayTracer.Program.Main();
            });
            Source = ProduceBitmap(bitmap);
        }
        private static WriteableBitmap ProduceBitmap(int[,,] source)
        {
            int height = source.GetLength(0);
            int width = source.GetLength(1);
            WriteableBitmap writeableBitmap = new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            writeableBitmap.Lock();

            unsafe
            {
                byte* basePtr = (byte*)writeableBitmap.BackBuffer;
                int stride = writeableBitmap.BackBufferStride;

                for (int y = 0; y < height; y++)
                {
                    byte* rowPtr = basePtr + y * stride;

                    for (int x = 0; x < width; x++)
                    {
                        int r = source[y, x, 0];
                        int g = source[y, x, 1];
                        int b = source[y, x, 2];

                        rowPtr[x * 4 + 0] = (byte)b;
                        rowPtr[x * 4 + 1] = (byte)g;
                        rowPtr[x * 4 + 2] = (byte)r;
                        rowPtr[x * 4 + 3] = 255;
                    }
                }
            }

            writeableBitmap.AddDirtyRect(
                new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight)
            );

            writeableBitmap.Unlock();

            return writeableBitmap;

        }
    }
}

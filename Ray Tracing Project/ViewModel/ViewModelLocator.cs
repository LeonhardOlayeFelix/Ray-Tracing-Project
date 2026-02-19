using Microsoft.Extensions.DependencyInjection;
using Ray_Tracing_Project.IOCContainer;
using Ray_Tracing_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray_Tracing_Project.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();
        public ViewModelLocator()
        {
            Initialise();
        }

        public void Initialise()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainViewModel>();
            Ioc.Default = services.BuildServiceProvider();
        }
    }
}

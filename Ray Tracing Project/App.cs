using Microsoft.Extensions.DependencyInjection;
using Ray_Tracing_Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ray_Tracing_Project
{
    public class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainView mainView = new MainView();
            mainView.ShowDialog();
        }
    }
}

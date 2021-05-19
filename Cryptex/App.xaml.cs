using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Cryptex.ViewModels;

namespace Cryptex
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<ConfigureContainer>();
            Container = containerBuilder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                Window startWindow = new MainWindow();
                startWindow.DataContext = scope.Resolve<MainViewModel>();
                startWindow.Show();
            }

            base.OnStartup(e);
        }
    }
}

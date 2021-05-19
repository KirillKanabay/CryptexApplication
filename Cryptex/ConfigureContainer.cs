using System.Collections.Generic;
using Autofac;
using Cryptex.ViewModels;

namespace Cryptex
{
    internal class ConfigureContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterViewModels(builder);
            RegisterServices(builder);
            RegisterModels(builder);
            RegisterViews(builder);
        }

        private void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<KeysViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SecureFileViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SecureMessagesViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DiffieHellmanDemoViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RsaDemoViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AboutProgramViewModel>().AsSelf().InstancePerDependency();

            builder.Register(c => new MainViewModel()
            {
                //Регистрация навигационных VM
                NavigationViewModels = new Dictionary<string, BaseViewModel>()
                {
                    {"Ключи", c.Resolve<KeysViewModel>()},
                    {"Работа с текстом", c.Resolve<SecureMessagesViewModel>()},
                    {"Работа с файлами", c.Resolve<SecureFileViewModel>()},
                    {"Демонстрация RSA", c.Resolve<RsaDemoViewModel>()},
                    {"Демонстрация DH", c.Resolve<DiffieHellmanDemoViewModel>()},
                    {"О программе", c.Resolve<AboutProgramViewModel>()},
                }
            });
        }

        private void RegisterServices(ContainerBuilder builder)
        {

        }

        private void RegisterModels(ContainerBuilder builder)
        {

        }

        private void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
        }
    }
}

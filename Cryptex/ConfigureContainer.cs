using System.Collections.Generic;
using Autofac;
using Cryptex.Models;
using Cryptex.Services;
using Cryptex.Services.Helpers;
using Cryptex.ViewModels;
using Cryptex.ViewModels.RsaDemoViewModels;

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

            #region RSA Demo

            builder.RegisterType<RsaDemoViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaInfoViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaSetPQViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaCalculateViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaEncryptViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaDecryptViewModel>().AsSelf().InstancePerDependency();

            #endregion

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
            builder.RegisterType<DemoRsaCryptography>().As<IDemoRsaCryptography>().InstancePerDependency();
            builder.RegisterType<PrimeNumbersWorker>().As<IPrimeNumbersWorker>().InstancePerLifetimeScope();
            builder.RegisterType<GcdNumbersWorker>().As<IGcdNumbersWorker>().InstancePerLifetimeScope();
        }

        private void RegisterModels(ContainerBuilder builder)
        {
            builder.RegisterType<RsaInfoModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RsaDemoModel>().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
        }
    }
}

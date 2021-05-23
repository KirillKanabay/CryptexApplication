using System.Collections.Generic;
using Autofac;
using Cryptex.Helpers;
using Cryptex.Models;
using Cryptex.Services;
using Cryptex.Services.DiffieHellman;
using Cryptex.Services.Hash;
using Cryptex.Services.Helpers;
using Cryptex.Services.RSA;
using Cryptex.ViewModels;
using Cryptex.ViewModels.DhDemoViewModels;
using Cryptex.ViewModels.RsaCryptography;
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
            builder.RegisterType<KeysViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<KeyEditorViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<SelectableKeyViewModel>().AsSelf().InstancePerDependency();

            builder.RegisterType<SecureFileViewModel>().AsSelf().InstancePerDependency();

            #region SecureMessages

            builder.RegisterType<SecureMessagesViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<EncryptMessageViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<DecryptMessageViewModel>().AsSelf().InstancePerDependency();

            #endregion

            #region SecureFiles

            builder.RegisterType<SecureFileViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<EncryptFileViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<DecryptFileViewModel>().AsSelf().InstancePerDependency();

            #endregion

            #region RSA Demo

            builder.RegisterType<RsaDemoViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaSetPQViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaCalculateViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaEncryptViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StepperRsaDecryptViewModel>().AsSelf().InstancePerDependency();

            #endregion

            #region Diffie-Hellman Demo

            builder.RegisterType<DiffieHellmanDemoViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StepperDhCalculateGeneralKeyViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StepperDhCalculatePublicKeysViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StepperDhEncryptDecryptViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StepperDhSetPGViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StepperDhSetPrivateKeysViewModel>().AsSelf().InstancePerLifetimeScope();
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
            builder.RegisterType<ViewModelContainer>().As<IViewModelContainer>().SingleInstance();
            
            builder.RegisterType<RsaKeyFileWorker>().As<IRsaKeyFileWorker>().SingleInstance();
            builder.RegisterType<RsaCryptography>().As<IRsaCryptography>().SingleInstance();
            builder.RegisterType<Md5Calculator>().As<IHashCalculator>().SingleInstance();

            builder.RegisterType<DemoRsaCryptography>().As<IDemoRsaCryptography>().InstancePerDependency();
            builder.RegisterType<PrimeNumbersWorker>().As<IPrimeNumbersWorker>().InstancePerLifetimeScope();
            builder.RegisterType<GcdNumbersWorker>().As<IGcdNumbersWorker>().InstancePerLifetimeScope();
            builder.RegisterType<DemoDHCryptography>().As<DemoDHCryptography>().InstancePerLifetimeScope();
            
        }

        private void RegisterModels(ContainerBuilder builder)
        {
            builder.RegisterType<RsaModel>().AsSelf().SingleInstance();
            builder.RegisterType<RsaDemoModel>().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
        }
    }
}

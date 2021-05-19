using Autofac;

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

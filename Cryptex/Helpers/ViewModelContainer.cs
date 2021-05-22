using Autofac;
using Cryptex.ViewModels;

namespace Cryptex.Helpers
{
    public class ViewModelContainer : IViewModelContainer
    {
        private readonly ILifetimeScope _scope;
        public ViewModelContainer(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return _scope.Resolve<TViewModel>();
        }
    }
}

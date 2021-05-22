using Cryptex.ViewModels;

namespace Cryptex.Helpers
{
    public interface IViewModelContainer
    {
        TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel;
    }
}
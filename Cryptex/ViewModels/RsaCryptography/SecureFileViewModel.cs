using Cryptex.Helpers;

namespace Cryptex.ViewModels.RsaCryptography
{
    class SecureFileViewModel:BaseViewModel
    {
        private readonly IViewModelContainer _viewModelContainer;

        public SecureFileViewModel(IViewModelContainer viewModelContainer)
        {
            _viewModelContainer = viewModelContainer;
        }

        public BaseViewModel EncryptFileViewModel =>
            _viewModelContainer.GetViewModel<EncryptFileViewModel>();

        public BaseViewModel DecryptFileViewModel =>
            _viewModelContainer.GetViewModel<DecryptFileViewModel>();
    }
}

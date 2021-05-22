using Cryptex.Helpers;

namespace Cryptex.ViewModels.RsaCryptography
{
    class SecureMessagesViewModel:BaseViewModel
    {
        private readonly IViewModelContainer _viewModelContainer;

        public SecureMessagesViewModel(IViewModelContainer viewModelContainer)
        {
            _viewModelContainer = viewModelContainer;
        }

        public BaseViewModel EncryptMessageViewModel =>
            _viewModelContainer.GetViewModel<EncryptMessageViewModel>();

        public BaseViewModel DecryptMessageViewModel =>
            _viewModelContainer.GetViewModel<DecryptMessageViewModel>();
    }
}

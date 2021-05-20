using Cryptex.ViewModels;

namespace Cryptex.Views.Dialogs
{
    /// <summary>
    /// Диалог сообщения на вход которого приходит VM
    /// </summary>
    class MessageDialogViewModel:BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MessageDialogViewModel()
        {
        }
        public MessageDialogViewModel(BaseViewModel viewModel)
        {
            SelectedViewModel = viewModel;
        }
    }
}

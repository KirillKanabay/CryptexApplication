using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Validators;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaDecryptViewModel:BaseViewModel
    {
        private readonly RsaDemoModel _rsaDemoModel;

        private bool _finishButtonIsEnabled;
        private string _plainText;

        public StepperRsaDecryptViewModel(RsaDemoModel rsaDemoModel)
        {
            _rsaDemoModel = rsaDemoModel;
        }

        public bool FinishButtonIsEnabled
        {
            get => _finishButtonIsEnabled;
            set
            {
                _finishButtonIsEnabled = value;
                OnPropertyChanged(nameof(FinishButtonIsEnabled));
            }
        }
        
        public string PlainText
        {
            get => _rsaDemoModel.DecryptedText;
            set
            {
                _rsaDemoModel.DecryptedText = value;
                OnPropertyChanged(nameof(PlainText));
            }
        }

        public string EncryptedText => _rsaDemoModel.EncryptedMessage;

        #region Команды

        public AsyncRelayCommand DecryptCommand => new AsyncRelayCommand(EncryptMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        private async Task EncryptMethod(object arg)
        {
            FinishButtonIsEnabled = false;
            PlainText = await Task.Run(() => _rsaDemoModel.PlainText);
            FinishButtonIsEnabled = true;
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            await DialogHost.Show(view, "DialogRoot");
        }
        #endregion
        
    }
}

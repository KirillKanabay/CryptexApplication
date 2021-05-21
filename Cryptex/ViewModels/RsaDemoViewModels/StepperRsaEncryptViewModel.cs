using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Validators;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Expression.Interactivity.Media;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaEncryptViewModel :BaseViewModel, IDataErrorInfo
    {
        private readonly RsaDemoModel _rsaDemoModel;
        
        private bool _continueButtonIsEnabled;
        private bool _isProcessing;
        private string _plainText;
        private string _encryptedText;

        public StepperRsaEncryptViewModel(RsaDemoModel rsaDemoModel)
        {
            _rsaDemoModel = rsaDemoModel;
        }

        public bool ContinueButtonIsEnabled
        {
            get => _continueButtonIsEnabled;
            set
            {
                _continueButtonIsEnabled = value;
                OnPropertyChanged(nameof(ContinueButtonIsEnabled));
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
            }
        }

        public string PlainText
        {
            get => _plainText;
            set
            {
                _plainText = value;
                OnPropertyChanged(nameof(PlainText));
            }
        }

        public string EncryptedText
        {
            get => _encryptedText;
            set
            {
                _encryptedText = value;
                OnPropertyChanged(nameof(EncryptedText));
            }
        }

        #region Команды

        public AsyncRelayCommand EncryptCommand => new AsyncRelayCommand(EncryptMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        private async Task EncryptMethod(object arg)
        {
            IsProcessing = true;
            ContinueButtonIsEnabled = false;
            EncryptedText = await _rsaDemoModel.Encrypt(PlainText);
            IsProcessing = false;
            ContinueButtonIsEnabled = true;
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            var result = await DialogHost.Show(view, "DialogRoot");
        }
        #endregion


        public bool ButtonIsEnabled => !_errors.Any();

        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        public string Error { get; }

        public string this[string columnName]{
            get
            {
                string error = columnName switch
                {
                    nameof(PlainText) => new Validation(new NotEmptyFieldValidationRule(PlainText)).Validate(),
                    _ => null
                };
                _errors.Remove(columnName);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    _errors.Add(columnName, error);
                }

                OnPropertyChanged(nameof(ButtonIsEnabled));
                //OnPropertyChanged(nameof(SetupButtonIsEnabled));
                return error;
            }
        }
    }
}

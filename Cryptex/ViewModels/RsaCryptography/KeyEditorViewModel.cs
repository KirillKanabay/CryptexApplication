using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cryptex.Helpers;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Validators;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaCryptography
{
    class KeyEditorViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Поля

        private readonly RsaModel _rsaModel;

        private Visibility _progressBarVisibility = Visibility.Collapsed;

        #endregion

        #region Конструкторы

        public KeyEditorViewModel(RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
        }

        #endregion

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _length;

        public string Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                _progressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }

        #region Команды

        public AsyncRelayCommand SaveItem => new AsyncRelayCommand(SaveMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        #endregion

        #region Методы

        private async Task SaveMethod(object parameter)
        {
            ProgressBarVisibility = Visibility.Visible;

            await _rsaModel.AddAsync(new RsaKeyCryptography(int.Parse(_length), TrueFileName.Get(Name)));

            ProgressBarVisibility = Visibility.Collapsed;

            string snackbarMessage = $"Ключ \"{Name}\" успешно добавлен.";

            SendSnackbar(snackbarMessage);
            CloseCurrentDialog();
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty) o)
            };
            await DialogHost.Show(view, "EditorDialog");
        }

        #endregion

        #region Валидация

        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        public bool SaveButtonIsEnabled => !_errors.Any();
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                var error = columnName switch
                {
                    nameof(Name) => new Validation(new NotEmptyFieldValidationRule(Name)).Validate(),
                    nameof(Length) => new Validation(new NotEmptyFieldValidationRule(Length),
                        new NumberValidationRule(Length), new RangeNumberValidationRule(Length, 512, 4096)).Validate(),
                    _ => null
                };

                _errors.Remove(columnName);
                if (!string.IsNullOrWhiteSpace(error))
                    _errors.Add(columnName, error);
                OnPropertyChanged(nameof(SaveButtonIsEnabled));
                return error;
            }
        }

        #endregion
    }
}
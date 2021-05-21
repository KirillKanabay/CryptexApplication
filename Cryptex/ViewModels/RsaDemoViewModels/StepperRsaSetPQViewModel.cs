using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Validators;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaSetPQViewModel : BaseViewModel, IDataErrorInfo
    {
        private string _p;
        private string _q;
        private readonly RsaDemoModel _rsaDemoModel;
        private bool _isSetup;

        private Visibility _progressBarVisibility = Visibility.Collapsed;
        
        public StepperRsaSetPQViewModel(RsaDemoModel rsaDemoModel)
        {
            _rsaDemoModel = rsaDemoModel;
        }

        public string P
        {
            get => _p;
            set
            {
                _p = value;
                OnPropertyChanged(nameof(P));
            }
        }

        public string Q
        {
            get => _q;
            set
            {
                _q = value;
                OnPropertyChanged(nameof(Q));
            }
        }

        public bool SetupButtonIsEnabled => !_errors.Any();
        public bool ButtonIsEnabled => !_errors.Any() && _isSetup;

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

        public AsyncRelayCommand Setup => new AsyncRelayCommand(SetupMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        public AsyncRelayCommand GenerateP => new AsyncRelayCommand(GeneratePMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand GenerateQ => new AsyncRelayCommand(GenerateQMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        private async Task GeneratePMethod(object arg)
        {
            await _rsaDemoModel.GenerateP();
            P = _rsaDemoModel.P.ToString();
        }

        private async Task GenerateQMethod(object arg)
        {
            await _rsaDemoModel.GenerateQ();
            Q = _rsaDemoModel.Q.ToString();
        }

        private async Task SetupMethod(object arg)
        {
            ProgressBarVisibility = Visibility.Visible;
            
            await _rsaDemoModel.SetP(long.Parse(P));
            await _rsaDemoModel.SetQ(long.Parse(Q));
            await _rsaDemoModel.Calculate();

            _isSetup = true;
            OnPropertyChanged(nameof(ButtonIsEnabled));

            ProgressBarVisibility = Visibility.Collapsed;

            SendSnackbar($"Ключ P: {P} и Q: {Q} установлены.");
        }

        public async void ExecuteRunDialog(object o)
        {
            CloseCurrentDialog();
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty) o)
            };
            var result = await DialogHost.Show(view, "RootDialog");
        }

        #endregion


        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                string error = columnName switch
                {
                    nameof(P) => new Validation(new NotEmptyFieldValidationRule(P), new NumberValidationRule(P), new RangeNumberValidationRule(P, 1000u, 1000000u),
                        new PrimeNumberValidationRule(P)).Validate(),
                    nameof(Q) => new Validation(new NotEmptyFieldValidationRule(Q), new NumberValidationRule(Q), new RangeNumberValidationRule(Q, 1000u, 1000000u),
                        new PrimeNumberValidationRule(Q)).Validate(),
                    _ => null
                };
                _errors.Remove(columnName);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    _errors.Add(columnName, error);
                }

                if (P == Q)
                {
                    if(!_errors.ContainsKey(nameof(P)))
                        _errors.Add("P", "Числа не могут быть одинаковыми");
                    if (!_errors.ContainsKey(nameof(Q)))
                        _errors.Add("Q", "Числа не могут быть одинаковыми");
                }

                if (P != Q)
                {
                    if (_errors.ContainsKey(nameof(P)) && _errors[nameof(P)] == "Числа не могут быть одинаковыми")
                        _errors.Remove("P");
                    if (_errors.ContainsKey(nameof(Q)) && _errors[nameof(Q)] == "Числа не могут быть одинаковыми")
                        _errors.Remove("Q");
                }
                OnPropertyChanged(nameof(ButtonIsEnabled));
                OnPropertyChanged(nameof(SetupButtonIsEnabled));
                return error;
            }
        }
    }
}
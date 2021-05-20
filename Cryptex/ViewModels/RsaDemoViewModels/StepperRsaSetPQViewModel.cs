using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cryptex.Models;
using Cryptex.Validators;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaSetPQViewModel :BaseViewModel, IDataErrorInfo
    {
        private string _p;
        private string _q;
        private readonly RsaDemoModel _rsaDemoModel;
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

        public bool ButtonIsEnabled
        {
            get
            {
                if (P == Q)
                {
                    return false;
                }

                if (!_errors.Any())
                {
                    _rsaDemoModel.DemoRsa.PSet(ulong.Parse(P));
                    _rsaDemoModel.DemoRsa.QSet(ulong.Parse(Q));
                    
                    return true;
                }

                return false;
            }
        }

        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                string error = columnName switch
                {
                    nameof(P) => new Validation(new NotEmptyFieldValidationRule(P), new NumberValidationRule(P), new PrimeNumberValidationRule(P)).Validate(), 
                    nameof(Q) => new Validation(new NotEmptyFieldValidationRule(Q), new NumberValidationRule(Q), new PrimeNumberValidationRule(Q)).Validate(),
                    _ => null
                };
                _errors.Remove(columnName);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    _errors.Add(columnName, error);
                }
                    
                OnPropertyChanged(nameof(ButtonIsEnabled));
                return error;
            }
        }
    }
}

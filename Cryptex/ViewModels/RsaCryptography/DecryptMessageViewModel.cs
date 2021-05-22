using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Cryptex.ViewModels.RsaCryptography
{
    public class DecryptMessageViewModel:BaseViewModel
    {
        #region Поля

        private readonly RsaModel _rsaModel;
        private RsaKeyCryptography _key;

        private List<RsaKeyCryptography> _keysCollection;

        private Visibility _keyNameVisibility = Visibility.Collapsed;

        private bool _plainTextCardIsEnabled;
        private string _plainText;

        private Visibility _decryptProgressBarVisibility = Visibility.Collapsed;

        private bool _encryptTextCardIsEnabled;
        private string _encryptedText;
        #endregion

        #region Конструкторы
        public DecryptMessageViewModel(RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
        }
        #endregion

        #region Свойства

        public List<RsaKeyCryptography> KeysCollection
        {
            get => _keysCollection;
            set
            {
                _keysCollection = value;
                OnPropertyChanged(nameof(KeysCollection));
            }
        }

        private int _selectedKeyIndex;

        public int SelectedKeyIndex
        {
            get => _selectedKeyIndex;
            set
            {
                _selectedKeyIndex = value;
                OnPropertyChanged(nameof(SelectedKeyIndex));
            }
        }

        public RsaKeyCryptography SelectedKey
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(SelectedKey));
                OnPropertyChanged(nameof(KeyName));
            }
        }
        public string KeyName => _key?.Name;

        public Visibility KeyNameVisibility
        {
            get => _keyNameVisibility;
            set
            {
                _keyNameVisibility = value;
                OnPropertyChanged(nameof(KeyNameVisibility));
            }
        }

        public bool PlainTextCardIsEnabled
        {
            get => _plainTextCardIsEnabled;
            set
            {
                _plainTextCardIsEnabled = value;
                OnPropertyChanged(nameof(PlainTextCardIsEnabled));
            }
        }

        public string PlainText
        {
            get => _plainText;
            set
            {
                _plainText = value;
                OnPropertyChanged(nameof(PlainText));
                OnPropertyChanged(nameof(DecryptButtonIsEnabled));
            }
        }

        public bool DecryptButtonIsEnabled => EncryptedText?.Trim().Length > 0;

        public Visibility DecryptProgressBarVisibility
        {
            get => _decryptProgressBarVisibility;
            set
            {
                _decryptProgressBarVisibility = value;
                OnPropertyChanged(nameof(DecryptProgressBarVisibility));
            }
        }

        public bool EncryptedTextCardIsEnabled
        {
            get => _encryptTextCardIsEnabled;
            set
            {
                _encryptTextCardIsEnabled = value;
                OnPropertyChanged(nameof(EncryptedTextCardIsEnabled));
            }
        }

        public string EncryptedText
        {
            get => _encryptedText;
            set
            {
                _encryptedText = value;
                OnPropertyChanged(nameof(EncryptedText));
                OnPropertyChanged(nameof(DecryptButtonIsEnabled));
            }
        }
        #endregion

        #region Команды

        public AsyncRelayCommand LoadedCommand => new AsyncRelayCommand(ImportKeysMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand Decrypt => new AsyncRelayCommand(DecryptMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        #endregion

        #region Методы

        private async Task ImportKeysMethod(object arg)
        {
            KeysCollection = await _rsaModel.LoadAsync();
            KeyNameVisibility = Visibility.Visible;
            EncryptedTextCardIsEnabled = true;
        }

        private async Task DecryptMethod(object arg)
        {
            DecryptProgressBarVisibility = Visibility.Visible;
            PlainTextCardIsEnabled = false;
            try
            {
                PlainText = await _rsaModel.Decrypt(EncryptedText, _key);
                PlainTextCardIsEnabled = true;
            }
            finally
            {
                DecryptProgressBarVisibility = Visibility.Collapsed;
            }
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            await DialogHost.Show(view, "RootDialog");
        }
        #endregion
    }
}

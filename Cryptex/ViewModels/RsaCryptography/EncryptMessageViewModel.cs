using System;
using System.Threading.Tasks;
using System.Windows;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Cryptex.ViewModels.RsaCryptography
{
    class EncryptMessageViewModel:BaseViewModel
    {
        #region Поля

        private readonly RsaModel _rsaModel;
        private RsaKeyCryptography _key;
        
        private Visibility _importProgressBarVisibility = Visibility.Collapsed;
        private Visibility _keyNameVisibility = Visibility.Collapsed;

        private bool _plainTextCardIsEnabled;
        private string _plainText;
        private Visibility _encryptProgressBarVisibility = Visibility.Collapsed;

        private bool _encryptedTextCardIsEnabled;
        private string _encryptedText;
        #endregion

        #region Конструкторы
        public EncryptMessageViewModel(RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
        }
        #endregion

        #region Свойства

        public string KeyName => _key?.Name;
        
        public Visibility ImportProgressBarVisibility
        {
            get => _importProgressBarVisibility;
            set
            {
                _importProgressBarVisibility = value;
                OnPropertyChanged(nameof(ImportProgressBarVisibility));
            }
        }

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
                OnPropertyChanged(nameof(EncryptButtonIsEnabled));
            }
        }

        public bool EncryptButtonIsEnabled => PlainText?.Trim().Length > 0;

        public Visibility EncryptProgressBarVisibility
        {
            get => _encryptProgressBarVisibility;
            set
            {
                _encryptProgressBarVisibility = value;
                OnPropertyChanged(nameof(EncryptProgressBarVisibility));
            }
        }

        public bool EncryptedTextCardIsEnabled
        {
            get => _encryptedTextCardIsEnabled;
            set
            {
                _encryptedTextCardIsEnabled = value;
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
            }
        }
        #endregion

        #region Команды

        public AsyncRelayCommand ImportKey => new AsyncRelayCommand(ImportKeyMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand Encrypt => new AsyncRelayCommand(EncryptMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        #endregion


        #region Методы

        private async Task ImportKeyMethod(object arg)
        {
            var openFileDialog = new OpenFileDialog {Filter = "RSA ключ (*.ck)|*.ck"};

            if (openFileDialog.ShowDialog() == false)
                return;

            try
            {
                ImportProgressBarVisibility = Visibility.Visible;

                KeyNameVisibility = Visibility.Collapsed;
                
                _key = await _rsaModel.ImportKey(openFileDialog.FileName);
                
                KeyNameVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(KeyName));

                PlainTextCardIsEnabled = true;
            }
            finally
            {
                ImportProgressBarVisibility = Visibility.Collapsed;
            }
        }

        private async Task EncryptMethod(object arg)
        {
            EncryptProgressBarVisibility = Visibility.Visible;
            EncryptedTextCardIsEnabled = false;
            try
            {
                EncryptedText = await _rsaModel.Encrypt(PlainText, _key);
                EncryptedTextCardIsEnabled = true;
            }
            finally
            {
                EncryptProgressBarVisibility = Visibility.Collapsed;
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

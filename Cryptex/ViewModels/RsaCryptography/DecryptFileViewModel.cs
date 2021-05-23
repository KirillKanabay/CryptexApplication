using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Cryptex.ViewModels.RsaCryptography
{
    class DecryptFileViewModel:BaseViewModel
    {
        #region Поля

        private readonly RsaModel _rsaModel;
        private RsaKeyCryptography _key;

        private List<RsaKeyCryptography> _keysCollection;

        private Visibility _keyNameVisibility = Visibility.Collapsed;

        private static readonly Color GrayColor = Color.FromArgb(a: 180, r: 115, g: 115, b: 115); 
        private static readonly Color GreenColor = Color.FromArgb(a: 180, r: 000, g: 184, b: 35); 
        private static readonly Color RedColor = Color.FromArgb(a: 180, r: 195, g: 33, b: 33); 

        #endregion

        #region Конструкторы
        public DecryptFileViewModel(RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
        }
        #endregion

        #region Свойства

        #region Импорт ключа

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
                OnPropertyChanged(nameof(EncryptFileCardIsEnabled));
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

        #endregion
        
        #region Импорт файла 

        private Visibility _importFileProgressBarVisibility = Visibility.Collapsed;
        private Visibility _fileInfoVisibility = Visibility.Collapsed;
        private Visibility _encryptProgressBarVisibility = Visibility.Collapsed;
        private string _fileName;
        private string _hashFile;
        private bool _encryptButtonIsEnabled;
        public Visibility EncryptProgressBarVisibility
        {
            get => _encryptProgressBarVisibility;
            set
            {
                _encryptProgressBarVisibility = value;
                OnPropertyChanged(nameof(EncryptProgressBarVisibility));
            }
        }
        public Visibility ImportFileProgressBarVisibility
        {
            get => _importFileProgressBarVisibility;
            set
            {
                _importFileProgressBarVisibility = value;
                OnPropertyChanged(nameof(ImportFileProgressBarVisibility));
            }
        }
        public Visibility FileInfoVisibility
        {
            get => _fileInfoVisibility;
            set
            {
                _fileInfoVisibility = value;
                OnPropertyChanged(nameof(FileInfoVisibility));
            }
        }
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        public string FileHash
        {
            get => _hashFile;
            set
            {
                _hashFile = value;
                OnPropertyChanged(nameof(FileHash));
            }
        }

        public bool EncryptFileCardIsEnabled => KeyName?.Trim().Length > 0;
        public bool EncryptButtonIsEnabled
        {
            get => _encryptButtonIsEnabled;
            set
            {
                _encryptButtonIsEnabled = value;
                OnPropertyChanged(nameof(EncryptButtonIsEnabled));
            }
        }
        #endregion

        #region Подписанный файл

        private bool _signatureFileCardIsEnabled;
        private string _signatureText;
        private Visibility _importSignatureProgressBarVisibility = Visibility.Collapsed;
        private Visibility _checkSignatureProgressBarVisibility = Visibility.Collapsed;
        
        private string _verifyText = "Проверка не проводилась";
        private Brush _verifyColor = new SolidColorBrush(GrayColor);
        public bool SignatureFileCardIsEnabled
        {
            get => _signatureFileCardIsEnabled;
            set
            {
                _signatureFileCardIsEnabled = value;
                OnPropertyChanged(nameof(SignatureFileCardIsEnabled));
            }
        }
        public string SignatureText
        {
            get => _signatureText;
            set
            {
                _signatureText = value;
                OnPropertyChanged(nameof(SignatureText));
                OnPropertyChanged(nameof(CheckSignatureButtonIsEnabled));
            }
        }
        public bool CheckSignatureButtonIsEnabled => SignatureText?.Trim().Length > 0;

        public Visibility ImportSignatureProgressBarVisibility
        {
            get => _importSignatureProgressBarVisibility;
            set
            {
                _importSignatureProgressBarVisibility = value;
                OnPropertyChanged(nameof(ImportSignatureProgressBarVisibility));
            }
        }
        public Visibility CheckSignatureProgressBarVisibility
        {
            get => _checkSignatureProgressBarVisibility;
            set
            {
                _checkSignatureProgressBarVisibility = value;
                OnPropertyChanged(nameof(CheckSignatureButtonIsEnabled));
            }
        }

        public string VerifyText
        {
            get => _verifyText;
            set
            {
                _verifyText = value;
                OnPropertyChanged(nameof(VerifyText));
            }
        }

        public Brush VerifyColor
        {
            get => _verifyColor;
            set
            {
                _verifyColor = value;
                OnPropertyChanged(nameof(VerifyColor));
            }
        }
        #endregion

        #endregion

        #region Команды

        public AsyncRelayCommand LoadedCommand => new AsyncRelayCommand(ImportKeysMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand ImportFile => new AsyncRelayCommand(ImportFileMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand ImportSignature => new AsyncRelayCommand(ImportSignatureMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        public AsyncRelayCommand CheckSignature => new AsyncRelayCommand(CheckSignatureMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        #endregion

        #region Методы

        private async Task ImportFileMethod(object arg)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
                return;
            try
            {
                ImportFileProgressBarVisibility = Visibility.Visible;
                FileInfoVisibility = Visibility.Collapsed;
                SignatureFileCardIsEnabled = false;
                SignatureText = string.Empty;
                VerifyText = "Проверка не проводилась";
                VerifyColor = new SolidColorBrush(GrayColor);
                
                FileName = Path.GetFileName(openFileDialog.FileName);
                FileHash = await _rsaModel.CalculateHash(openFileDialog.FileName);

                FileInfoVisibility = Visibility.Visible;
                SignatureFileCardIsEnabled = true;
            }
            finally
            {
                ImportFileProgressBarVisibility = Visibility.Collapsed;
            }
        }

        private async Task ImportKeysMethod(object arg)
        {
            KeysCollection = await _rsaModel.LoadAsync();
            KeyNameVisibility = Visibility.Visible;
        }

        private async Task ImportSignatureMethod(object arg)
        {
            var openFileDialog = new OpenFileDialog { Filter = "Подпись файла RSA ключом (*.sck)|*.sck" };

            if (openFileDialog.ShowDialog() == false)
                return;

            try
            {
                ImportSignatureProgressBarVisibility = Visibility.Visible;

                KeyNameVisibility = Visibility.Collapsed;

                SignatureText = await File.ReadAllTextAsync(openFileDialog.FileName);

                KeyNameVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(KeyName));
            }
            finally
            {
                ImportSignatureProgressBarVisibility = Visibility.Collapsed;
            }
        }

        private async Task CheckSignatureMethod(object arg)
        {
            bool isVerify = await _rsaModel.CheckSignature(_key, SignatureText, FileHash);
            VerifyText = isVerify ? "Файл подтвержден" : "Файл не подтвержден";
            VerifyColor = isVerify ? new SolidColorBrush(GreenColor) : new SolidColorBrush(RedColor);
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

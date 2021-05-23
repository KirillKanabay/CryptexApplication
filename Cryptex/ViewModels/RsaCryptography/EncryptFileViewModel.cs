using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cryptex.Helpers;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Cryptex.ViewModels.RsaCryptography
{
    class EncryptFileViewModel : BaseViewModel
    {
        #region Поля

        private readonly RsaModel _rsaModel;
        
        #endregion

        #region Конструкторы

        public EncryptFileViewModel(RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
        }

        #endregion

        #region Свойства

        #region Импорт ключа
        private RsaKeyCryptography _key;
        private Visibility _importProgressBarVisibility = Visibility.Collapsed;
        private Visibility _keyNameVisibility = Visibility.Collapsed;
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

        #endregion

        #region Импорт файла 

        private Visibility _importFileProgressBarVisibility = Visibility.Collapsed;
        private Visibility _fileInfoVisibility = Visibility.Collapsed;
        private Visibility _encryptProgressBarVisibility = Visibility.Collapsed;
        private string _fileName;
        private string _hashFile;
        private bool _encryptFileCardIsEnabled;
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
        public bool EncryptFileCardIsEnabled
        {
            get => _encryptFileCardIsEnabled;
            set
            {
                _encryptFileCardIsEnabled = value;
                OnPropertyChanged(nameof(EncryptFileCardIsEnabled));
            }
        }
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
        private string _encryptedText;

        private bool _encryptedFileCardIsEnabled;
        public bool EncryptedFileCardIsEnabled
        {
            get => _encryptedFileCardIsEnabled;
            set
            {
                _encryptedFileCardIsEnabled = value;
                OnPropertyChanged(nameof(EncryptedFileCardIsEnabled));
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

        #region Экспорт подписи

        private Visibility _exportProgressBarVisibility = Visibility.Collapsed;

        public Visibility ExportProgressBarVisibility
        {
            get => _exportProgressBarVisibility;
            set
            {
                _exportProgressBarVisibility = value;
                OnPropertyChanged(nameof(ExportProgressBarVisibility));
            }
        }

        #endregion
        #endregion

        #region Команды

        public AsyncRelayCommand ImportKey => new AsyncRelayCommand(ImportKeyMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        public AsyncRelayCommand ImportFile => new AsyncRelayCommand(ImportFileMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        public AsyncRelayCommand Encrypt => new AsyncRelayCommand(EncryptMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        public AsyncRelayCommand ExportSignature => new AsyncRelayCommand(ExportSignatureMethod,
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
                EncryptButtonIsEnabled = false;
                EncryptedFileCardIsEnabled = false;
                EncryptedText = string.Empty;

                FileName = Path.GetFileName(openFileDialog.FileName);
                FileHash = await _rsaModel.CalculateHash(openFileDialog.FileName);

                FileInfoVisibility = Visibility.Visible;
                EncryptButtonIsEnabled = true;
            }
            finally
            {
                ImportFileProgressBarVisibility = Visibility.Collapsed;
            }
        }

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

                EncryptFileCardIsEnabled = true;
            }
            finally
            {
                ImportProgressBarVisibility = Visibility.Collapsed;
            }
        }

        private async Task EncryptMethod(object arg)
        {
            EncryptProgressBarVisibility = Visibility.Visible;
            EncryptedFileCardIsEnabled = false;
            try
            {
                EncryptedText = await _rsaModel.Encrypt(FileHash, _key);
                EncryptedFileCardIsEnabled = true;
            }
            finally
            {
                EncryptProgressBarVisibility = Visibility.Collapsed;
            }
        }

        private async Task ExportSignatureMethod(object arg)
        {
            try
            {
                ExportProgressBarVisibility = Visibility.Visible;
                string signatureFileName = TrueFileName.Get($"{FileName}_{DateTime.Now:G}");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Подпись файла RSA ключом (*.sck)|*.sck";
                sfd.FileName = signatureFileName;

                if (sfd.ShowDialog() == false)
                    return;
                await File.WriteAllTextAsync(sfd.FileName, EncryptedText, Encoding.Unicode);
            }
            finally
            {
                ExportProgressBarVisibility = Visibility.Collapsed;
            }
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty) o)
            };
            await DialogHost.Show(view, "RootDialog");
        }

        #endregion
    }
}
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaInfoViewModel : BaseViewModel
    {
        private readonly RsaInfoModel _rsaInfoModel;
        private FixedDocumentSequence _document;
        public StepperRsaInfoViewModel(RsaInfoModel rsaInfoModel)
        {
            _rsaInfoModel = rsaInfoModel;
        }

        public FixedDocumentSequence Document
        {
            get => _document;
            set
            {
                _document = value;
                OnPropertyChanged(nameof(Document));
            }
        }

        #region Команды

        public AsyncRelayCommand LoadedCommand => new AsyncRelayCommand(LoadDocument,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() {Title = "Ошибка", Message = ex.Message}); });

        #endregion

        private async Task LoadDocument(object arg)
        {
            Document = _rsaInfoModel.Load();
        }

        public async void ExecuteRunDialog(object o)
        {
            CloseCurrentDialog();
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            var result = await DialogHost.Show(view, "RootDialog");
        }
    }
}

using System.Threading.Tasks;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaCryptography
{
    class SelectableKeyViewModel:BaseViewModel
    {
        #region Поля

        private readonly RsaModel _rsaModel;
        private AsyncRelayCommand _removeCommand;

        private readonly RsaKeyCryptography _rkc;
        #endregion

        #region Конструкторы

        public SelectableKeyViewModel(RsaKeyCryptography rkc, RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
            _rkc = rkc;
        }

        #endregion

        #region Свойства

        public string Name => _rkc.Name;

        #endregion

        #region Диалоги

        public AsyncRelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??= new AsyncRelayCommand(ShowConfirmDeleteDialog,
                    (ex) =>
                    {
                        ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message });
                    });
            }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Показывает простой диалог сообщения
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            await DialogHost.Show(view, "RootDialog");
        }

        private MessageDialogProperty GetConfirmDeleteDialogProperty()
        {
            return new MessageDialogProperty()
            {
                Title = "Подтверждение удаления",
                Message = "Будет удален следующий ключ:" + Name
            };
        }

        /// <summary>
        /// Показывает диалог подтверждения удаления предмета
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task ShowConfirmDeleteDialog(object parameter)
        {
            var view = new MessageDialogOkCancel()
            {
                DataContext = new SampleMessageDialogViewModel(GetConfirmDeleteDialogProperty())
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingDeleteDialogEventHandler);
        }

        /// <summary>
        /// Обработчик события закрытия диалога подтверждения удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void ClosingDeleteDialogEventHandler(object sender, DialogClosingEventArgs eventargs)
        {
            if (Equals((eventargs.Parameter), true))
                _rsaModel.Delete(_rkc);
        }

        #endregion
    }
}

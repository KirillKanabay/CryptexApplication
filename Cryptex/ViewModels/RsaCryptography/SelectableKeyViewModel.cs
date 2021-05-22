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

        #endregion

        #region Конструкторы

        public SelectableKeyViewModel(RsaKeyCryptography rkc, RsaModel rsaModel)
        {
            _rsaModel = rsaModel;
            Rkc = rkc;
        }

        #endregion

        #region Свойства
        public RsaKeyCryptography Rkc { get; set; }

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

        private MessageDialogProperty GetConfirmDeleteDialogProperty()
        {
            return new MessageDialogProperty()
            {
                Title = "Подтверждение удаления",
                Message = "Будет удален следующий клиент:\n\n" + Rkc.Name
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
                _rsaModel.DeleteAsync(Customer);
        }

        #endregion
    }
}

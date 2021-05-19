using System.Windows;
using Cryptex.Helpers.Commands;

namespace Cryptex.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        #region Команды
        public DelegateCommand CloseWindow => new DelegateCommand((window) =>
        {
            ((Window)window).Close();
        });

        public DelegateCommand MinimizeWindow => new DelegateCommand((window) =>
        {
            ((Window)window).WindowState = WindowState.Minimized;
        });
        #endregion
    }
}

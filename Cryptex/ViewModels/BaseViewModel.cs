using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cryptex.ViewModels
{
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region DialogHost

        public virtual void CloseCurrentDialog()
        {
            var dialogHostCurrentSession = MainWindow.GetMainWindow?.Invoke().DialogHost.CurrentSession;
            dialogHostCurrentSession?.Close();
        }

        #endregion

        #region Snackbar
        public virtual void SendSnackbar(string message)
        {
            MainWindow.SendSnackbarAction?.Invoke(message);
        }

        #endregion
    }
}

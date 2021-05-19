using System.Collections.Generic;
using System.Windows;
using Cryptex.Helpers.Commands;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        #region Поля

        private BaseViewModel _currentViewModel;

        #endregion
        
        #region Свойства
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        /// <summary>
        /// ViewModels для навигации.
        /// Регистрация происходит здесь: <see cref="Cryptex.ConfigureContainer"/>
        /// </summary>
        public Dictionary<string, BaseViewModel> NavigationViewModels { get; set; }

        public List<INavigationItem> NavigationItems => new List<INavigationItem>()
        {
            new FirstLevelNavigationItem() { Label = "Ключи", Icon = PackIconKind.KeyChain, IsSelected = true },
            new FirstLevelNavigationItem() { Label = "Работа с текстом", Icon = PackIconKind.MessageTextLock},
            new FirstLevelNavigationItem() { Label = "Работа с файлами", Icon = PackIconKind.FileKey},
            new DividerNavigationItem(),
            new FirstLevelNavigationItem() { Label = "Демонстрация RSA", Icon = PackIconKind.EyeOutline},
            new FirstLevelNavigationItem() { Label = "Демонстрация DH", Icon = PackIconKind.EyeOutline},
            new DividerNavigationItem(),
            new FirstLevelNavigationItem() { Label = "О программе", Icon = PackIconKind.InformationOutline},
        };

        #endregion

        #region Команды
        public DelegateCommand SelectNavigationItemCommand => new DelegateCommand(parameter =>
        {
            if (parameter is WillSelectNavigationItemEventArgs args)
            {
                if (args.NavigationItemToSelect is FirstLevelNavigationItem navigationItem)
                {
                    CurrentViewModel = NavigationViewModels[navigationItem.Label];
                }
            }
        });

        public DelegateCommand CloseWindow => new DelegateCommand((window) =>
        {
            ((Window)window).Close();
        });

        public DelegateCommand MinimizeWindow => new DelegateCommand((window) =>
        {
            ((Window)window).WindowState = WindowState.Minimized;
        });

        public DelegateCommand LoadedCommand => new DelegateCommand(parameter =>
        {
            CurrentViewModel = NavigationViewModels["Ключи"];
        });
        #endregion
    }
}

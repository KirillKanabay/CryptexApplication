using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cryptex.Helpers;
using Cryptex.Helpers.Commands;
using Cryptex.Models;
using Cryptex.Services.RSA;
using Cryptex.Views.Dialogs;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace Cryptex.ViewModels.RsaCryptography
{
    class KeysViewModel:BaseViewModel
    {
        #region Поля

        private ObservableCollection<SelectableKeyViewModel> _keys;

        private readonly RsaModel _rsaModel;
        private readonly IViewModelContainer _viewModelContainer;

        private Visibility _progressBarVisibility;
        private string _filterText;

        #endregion

        #region Конструкторы

        public KeysViewModel(RsaModel rsaModel, IViewModelContainer viewModelContainer)
        {
            _rsaModel = rsaModel;
            _viewModelContainer = viewModelContainer;
            _rsaModel.RsaModelChanged += GetCollectionMethod;
        }

        #endregion

        #region Свойства

        public ObservableCollection<SelectableKeyViewModel> FilteredCollection
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_filterText))
                    return Filter();

                return _keys;
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                if (_filterText != null)
                {
                    OnPropertyChanged(nameof(FilteredCollection));
                }
            }
        }

        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                _progressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }

        #endregion

        #region Команды

        /// <summary>
        /// Команда получения коллекции предметов
        /// </summary>
        public AsyncRelayCommand GetItemsCollection => new AsyncRelayCommand(GetCollectionMethod,
            (ex) => { ExecuteRunDialog(new MessageDialogProperty() { Title = "Ошибка", Message = ex.Message }); });

        /// <summary>
        /// Команда открытия редактора
        /// </summary>
        public DelegateCommand OpenEditorDialog => new DelegateCommand(ExecuteRunEditorDialog);

        #endregion

        #region Методы

        private async Task GetCollectionMethod(object parameter)
        {
            ProgressBarVisibility = Visibility.Visible;
            _keys = GetSelectableViewModels(await _rsaModel.LoadAsync());
            OnPropertyChanged(nameof(FilteredCollection));
            ProgressBarVisibility = Visibility.Collapsed;
        }

        private ObservableCollection<SelectableKeyViewModel> GetSelectableViewModels(IEnumerable<RsaKeyCryptography> items)
        {
            var collection = new ObservableCollection<SelectableKeyViewModel>();
            foreach (var customer in items)
            {
                collection.Add(new SelectableKeyViewModel(customer, _rsaModel));
            }

            return collection;
        }

        private ObservableCollection<SelectableKeyViewModel> Filter()
        {
            var filteredCollection = _keys.Where(c =>
                c.Name.ToLower().Contains(_filterText.ToLower()));

            return new ObservableCollection<SelectableKeyViewModel>(filteredCollection);
        }

        /// <summary>
        /// Запуск редактора
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteRunEditorDialog(object o)
        {
            var view = new MessageDialogView()
            {
                DataContext = new MessageDialogViewModel(_viewModelContainer.GetViewModel<KeyEditorViewModel>())
            };

            await DialogHost.Show(view, "RootDialog");
        }

        public async void ExecuteRunDialog(object o)
        {
            var view = new SampleMessageDialog()
            {
                DataContext = new SampleMessageDialogViewModel((MessageDialogProperty)o)
            };
            await DialogHost.Show(view, "DialogRoot");
        }
        #endregion
    }
}

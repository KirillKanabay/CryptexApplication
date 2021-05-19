using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Cryptex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Func<MainWindow> GetMainWindow;
        public static Action<string> SendSnackbarAction;

        public MainWindow()
        {
            InitializeComponent();
            HeaderPanel.MouseDown += DragWindow;
            GetMainWindow += () => this;
            SendSnackbarAction += SendSnackbar;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == 0) { DragMove(); }
        }

        public void SendSnackbar(string message)
        {
            var messageQueue = Snackbar.MessageQueue;
            Task.Run(() => messageQueue?.Enqueue(message,
                "Закрыть",
                (param) => Trace.WriteLine("Закрыто"),
                message));
        }
    }
}
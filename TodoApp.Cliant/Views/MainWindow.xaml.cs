using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Refit;
using TodoApp.Client.Models;
using TodoApp.Client.Services;
using TodoApp.Client.ViewModels;
using TodoApp.Client.Views;


namespace TodoApp.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Client.ViewModels.MainViewModel _vm;
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            Loaded += async (_, _) => await vm.LoadAsync();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialogVm = _vm.CreateDialogVm();
            var dialog = new TodoDialog(dialogVm);

            if (dialog.ShowDialog() == true)
            {
                _ = _vm.LoadAsync();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is TodoItem item)
            {
                var vm = _vm.CreateDialogVm(item);
                var dialog = new TodoDialog(vm){ Owner = this };

                if (dialog.ShowDialog() == true)
                {
                    _ = _vm.LoadAsync();
                }
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _vm.LoadAsync();
        }

    }
}
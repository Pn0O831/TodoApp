using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoApp.Client.Models;
using TodoApp.Client.Services;
using TodoApp.Client.ViewModels;
using TodoApp.Client.Views;
using TodoApp.Client.Infrastructure;

namespace TodoApp.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ITodoApi _api;

        public ObservableCollection<TodoItem> Items { get; } = new();
        public ObservableCollection<TodoItem> FilteredItems { get; } = new();

        // フィルタ選択肢
        public List<string> FilterOptions { get; } = new() { "すべて", "完了", "未完了" };

        private string _selectedFilter = "すべて";
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        // 件数表示
        private string _statusText = $"";
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        // コマンド
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel(ITodoApi api)
        {
            _api = api;

            EditCommand = new RelayCommand<TodoItem>(Edit);
            DeleteCommand = new RelayCommand<TodoItem>(Delete);
        }

        // 一覧取得
        public async Task LoadAsync()
        {
            Items.Clear();
            var items = await _api.GetAllAsync();

            foreach (var item in items)
            {
                item.SetApi(_api);
                Items.Add(item);
            }

            ApplyFilter();
        }

        public TodoDialogViewModel CreateDialogVm(TodoItem? item = null)
        {
            return new TodoDialogViewModel(_api, item);
        }

        // フィルタ処理
        private void ApplyFilter()
        {
            FilteredItems.Clear();

            IEnumerable<TodoItem> query = Items;

            switch (SelectedFilter)
            {
                case "完了":
                    query = query.Where(x => x.IsCompleted);
                    break;
                case "未完了":
                    query = query.Where(x => !x.IsCompleted);
                    break;
            }

            foreach (var item in query)
                FilteredItems.Add(item);

            StatusText =
                $"全{Items.Count}件（完了: {Items.Count(i => i.IsCompleted)}件 / 未完了: {Items.Count(i => !i.IsCompleted)}件）";
        }

        // 編集
        private void Edit(TodoItem? item)
        {
            var dialogVm = new TodoDialogViewModel(_api, item);
            var dialog = new TodoDialog(dialogVm);

            if (dialog.ShowDialog() == true)
                _ = LoadAsync();
        }

        // 削除
        private async void Delete(TodoItem? item)
        {
            if (item == null) return;
            var result = System.Windows.MessageBox.Show(
                "削除しますか？",
                "確認",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                await _api.DeleteAsync(item.Id);
                await LoadAsync();
            }
        }


        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

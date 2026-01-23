using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TodoApp.Client.Models;
using TodoApp.Client.Services;

namespace TodoApp.Client.ViewModels
{
    public class TodoDialogViewModel : INotifyPropertyChanged
    {
        private readonly ITodoApi _api;

        public TodoItem EditingItem { get; set; }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public TodoDialogViewModel(ITodoApi api, TodoItem? item = null)
        {
            _api = api;
            EditingItem = item ?? new TodoItem();
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(EditingItem.Title))
            {
                ErrorMessage = ("タイトルは必須です");
                return false;
            }

            if (EditingItem.Id == 0)
            {
                await _api.CreateAsync(EditingItem);
            }
            else
            {
                await _api.UpdateAsync(EditingItem.Id, EditingItem);
            }

            return true;
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

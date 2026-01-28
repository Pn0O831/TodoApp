using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TodoApp.Client.Services;
using TodoApp.Client.Models;
using TodoApp.Client.Views;
using TodoApp.Client.ViewModels;

namespace TodoApp.Client.Models
{
    /// <summary>
    /// Todoアイテムのモデル
    /// </summary>
    public class TodoItem : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; } = 2; //1:高, 2:中, 3:低

        public string PriorityText => Priority switch
        {
            1 => "高",
            2 => "中",
            3 => "低",
            _ => "",
        };


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

//Models/TodoItem.cs

using System.ComponentModel.DataAnnotations;

namespace TodoApp.Server.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "タイトルは必須です")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime? DueDate { get; set; }

        [Range(1, 3)]
        public int Priority { get; set; } = 2;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}

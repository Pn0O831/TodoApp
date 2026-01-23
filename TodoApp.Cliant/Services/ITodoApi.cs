using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Client.Models;
using Refit;

// TodoApp.Cliant/Services/ITodoApi.cs
namespace TodoApp.Client.Services
{
    /// <summary>
    /// Refitインターフェース定義
    /// </summary>
    public interface ITodoApi
    {
        [Get("/todoitems")]
        Task<List<TodoItem>> GetAllAsync([Query] bool? isCompleted = null);


        [Get("/todoitems/{id}")]
        Task<TodoItem> GetByIdAsync(int id);


        [Post("/todoitems")]
        Task<TodoItem> CreateAsync([Body] TodoItem item);


        [Put("/todoitems/{id}")]
        Task UpdateAsync(int id, [Body] TodoItem item);


        [Delete("/todoitems/{id}")]
        Task DeleteAsync(int id);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Server.Data;
using TodoApp.Server.Models;

namespace TodoApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoDbContext _context;
        public TodoItemsController(TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(
            [FromQuery] bool? isCompleted,
            [FromQuery] string? keyword,
            [FromQuery] bool? onlyExprised,
            [FromQuery] bool? onlyActive)
        {
            var query = _context.TodoItems.AsQueryable();

            //LINQのWhereによる条件絞込み
            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            //タイトル部分一致
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(t => t.Title.Contains(keyword));
            }

            //期限切れ　(DueDateが現在日時より前)
            if (onlyExprised == true)
            {
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value < DateTime.Today);
            }

            //未完了
            if (onlyActive == true)
            {
                query = query.Where(t => !t.IsCompleted);
            }

            //射影
            var summaries = await _context.TodoItems
                .Select(t => new 
                {
                    t.Id,
                    t.Title,
                    t.IsCompleted
                })
                .ToListAsync();


            var items = await query
                .OrderBy(t => t.DueDate)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetByIdAsync(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }



        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateAsync([FromBody] TodoItem item)
        {
            
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                return BadRequest("Title is required.");
            }

            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existing = await _context.TodoItems.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.IsCompleted = item.IsCompleted;
            existing.DueDate = item.DueDate;
            existing.Priority = item.Priority;
            

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

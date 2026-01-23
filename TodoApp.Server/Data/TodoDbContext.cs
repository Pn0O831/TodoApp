using System.Collections.Generic;
using System.Reflection.Emit;
using TodoApp.Server.Models;
using Microsoft.EntityFrameworkCore;

//Data/TodoDbContext.cs

namespace TodoApp.Server.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options) { }


        public DbSet<TodoItem> TodoItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }
}

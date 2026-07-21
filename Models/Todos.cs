using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Todo.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Todos> Todos { get; set; }
        public TodoContext(DbContextOptions<TodoContext> op) : base(op)
        {
        }
    }
    public class Todos
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public string? Description { get; set; }
        // user
    }
}

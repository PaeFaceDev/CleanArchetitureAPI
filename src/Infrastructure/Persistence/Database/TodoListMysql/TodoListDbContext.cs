using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Update;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Database.TodoListMysql
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        {
            var db = this;
            db.Database.EnsureCreated();
        }

        public DbSet<Todolist> TodoList { get; set; }
    }
}
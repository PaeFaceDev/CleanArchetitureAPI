using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Create;
using Domain.Repositories.TodoList;
using Infrastructure.Persistence.Database.TodoListMysql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.TodoList
{
    public class GetTodoListRepositories : ITodoListRepositories
    {
        private readonly TodoListDbContext _todoListDbContext;
        public GetTodoListRepositories(TodoListDbContext todoListDbContext)
        {
            _todoListDbContext = todoListDbContext;
        }
        public async Task<List<Todolist>> GetTodolistMany()
        {
            var resulte = await _todoListDbContext.TodoList.Select(e => e).ToListAsync();
            return resulte;
        }
        public async Task<string> CreateTodoListAsync(TodoListCreate argument)
        {
            var dataInsert = new Todolist()
            {
                Category = argument.Category,
                Name = argument.Name
            };
            _todoListDbContext.TodoList.Add(dataInsert);
            await _todoListDbContext.SaveChangesAsync();

            return "Create Data Sccess";
        }
    }
}
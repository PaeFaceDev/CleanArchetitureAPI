using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Create;
using Domain.Entities.Update;
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

        public async Task<string> UpdateTodoList(TodoListUpdate filter)
        {
            var dataInsert = new Todolist()
            {
                Name = filter.Name
            };
            var resulte = await _todoListDbContext.TodoList.FirstOrDefaultAsync(id => id.Id == filter.Id);
            if (resulte == null)
            {
                return "Not Have Data on Database!!";
            }
            resulte.Name = filter.Name;
            await _todoListDbContext.SaveChangesAsync();
            return "Done.";
        }
    }
}
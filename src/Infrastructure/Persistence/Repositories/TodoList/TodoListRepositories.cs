using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Create;
using Domain.Entities.Delete;
using Domain.Entities.Update;
using Domain.Repositories.TodoList;
using Infrastructure.Persistence.Database.TodoListMysql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence.Repositories.TodoList
{
    public class TodoListRepositories : ITodoListRepositories
    {
        private readonly TodoListDbContext _todoListDbContext;
        private readonly IMemoryCache _cache;

        public TodoListRepositories(TodoListDbContext todoListDbContext, IMemoryCache cache)
        {
            _todoListDbContext = todoListDbContext;
            _cache = cache;
        }
        public async Task<List<Todolist>> GetTodolistMany()
        {
            string cacheKey = "todo:all";

            if (!_cache.TryGetValue(cacheKey, out List<Todolist> todos))
            {
                todos = await _todoListDbContext.TodoList
                    .ToListAsync();

                _cache.Set(cacheKey, todos, TimeSpan.FromMinutes(10));
            }
            if (todos == null)
            {
                todos = await _todoListDbContext.TodoList.ToListAsync();

                _cache.Set(cacheKey, todos, TimeSpan.FromMinutes(10));
            }
            return todos;
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

        public async Task<string> DeleteTodoList(DeleteTodoList filter)
        {
            var resual = await _todoListDbContext.TodoList.FindAsync(filter.Id);
            if (resual != null)
            {
                _todoListDbContext.TodoList.Remove(resual);
                await _todoListDbContext.SaveChangesAsync();
            }
            return "Done.";
        }
    }
}
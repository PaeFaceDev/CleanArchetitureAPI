using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories.TodoList;
using Infrastructure.Persistence.Database.TodoList;
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
        public async Task<string> CreateTodoListAsync(TodoListAggregate argument)
        {
            var dataInsert = new Todolist()
            {
                Category = argument.Category,
                Name = argument.Name
            };
            _todoListDbContext.TodoList.Add(dataInsert);
            await _todoListDbContext.SaveChangesAsync();
            
            if (dataInsert.Id.HasValue)
            {
                argument.SetId(dataInsert.Id.Value);
                argument.AddDomainEvent(new Domain.Events.TodoListCreatedEvent(dataInsert.Id.Value, argument.Name, argument.Category));
            }

            return "Create Data Success";
        }

        public async Task<string> UpdateTodoList(TodoListAggregate argument)
        {
            var resulte = await _todoListDbContext.TodoList.FirstOrDefaultAsync(id => id.Id == argument.Id);
            if (resulte == null)
            {
                return "Not Have Data on Database!!";
            }
            var oldName = resulte.Name ?? string.Empty;
            resulte.Name = argument.Name;
            await _todoListDbContext.SaveChangesAsync();
            
            argument.AddDomainEvent(new Domain.Events.TodoListUpdatedEvent(argument.Id, oldName, argument.Name, argument.Category));
            
            return "Done.";
        }

        public async Task<string> DeleteTodoList(TodoListAggregate argument)
        {
            var resual = await _todoListDbContext.TodoList.FindAsync(argument.Id);
            if (resual != null)
            {
                _todoListDbContext.TodoList.Remove(resual);
                await _todoListDbContext.SaveChangesAsync();
                
                argument.AddDomainEvent(new Domain.Events.TodoListDeletedEvent(argument.Id, argument.Name, argument.Category));
            }
            return "Done.";
        }
    }
}
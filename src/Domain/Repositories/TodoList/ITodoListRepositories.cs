using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Create;

namespace Domain.Repositories.TodoList
{
    public interface ITodoListRepositories
    {
        Task<List<Todolist>> GetTodolistMany();
        Task<string> CreateTodoListAsync(TodoListCreate argument);
    }
}
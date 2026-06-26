using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Create;
using Domain.Entities.Delete;
using Domain.Entities.Update;

namespace Domain.Repositories.TodoList
{
    public interface ITodoListRepositories
    {
        Task<List<Todolist>> GetTodolistMany();
        Task<string> CreateTodoListAsync(TodoListCreate argument);
        Task<string> UpdateTodoList(TodoListUpdate argument);
        Task<string> DeleteTodoList(DeleteTodoList argument);
    }
}
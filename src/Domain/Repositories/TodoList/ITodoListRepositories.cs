using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories.TodoList
{
    public interface ITodoListRepositories
    {
        Task<List<Todolist>> GetTodolistMany();
        Task<string> CreateTodoListAsync(TodoListAggregate argument);
        Task<string> UpdateTodoList(TodoListAggregate argument);
        Task<string> DeleteTodoList(TodoListAggregate argument);
    }
}
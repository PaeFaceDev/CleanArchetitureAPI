using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Domain.Repositories.TodoList;

namespace Application.UseCases.TodoList
{
    public class TodoListUseCase
    {
        private readonly ITodoListRepositories _todoListRepositories;
        public TodoListUseCase(ITodoListRepositories todoListRepositories)
        {
            _todoListRepositories = todoListRepositories;
        }

        public async Task<List<TodoListDtos>> GetTodoListDtosAsync()
        {
            var result = await _todoListRepositories.GetTodolistMany();
            return result.Select(e => new TodoListDtos()
            {
                Id = e.Id,
                Category = e.Category,
                Name = e.Name
            }).ToList();
        }
    }
}
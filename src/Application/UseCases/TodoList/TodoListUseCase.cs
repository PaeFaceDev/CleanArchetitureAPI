using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Dtos.TodoList.Insert;
using Domain.Entities.Create;
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

        public async Task<string> CreateTodoList(TodoListInsertDtos argument)
        {
            if (argument == null)
            {
                return "argument Not Found!";
            }
            var newArgument = new TodoListCreate
            {
                Category = argument.Category,
                Name = argument.Name
            };
            await _todoListRepositories.CreateTodoListAsync(newArgument);
            return "Done.";
        }


    }
}
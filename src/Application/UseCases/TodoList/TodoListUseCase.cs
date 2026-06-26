using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Dtos.TodoList.Delete;
using Application.Dtos.TodoList.Insert;
using Application.Dtos.TodoList.Update;
using Domain.Entities.Create;
using Domain.Entities.Update;
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

        public async Task<string> UpdateTodoList(TodoListUpdateDtos filter)
        {
            var filterArgument = new TodoListUpdate()
            {
                Name = filter.Name,
                Id = filter.Id
            };
            await _todoListRepositories.UpdateTodoList(filterArgument);
            return "Done.";
        }

        public async Task<string> DeleteTodoList(DeleteTodoListDtos filter)
        {

            await _todoListRepositories.DeleteTodoList(new Domain.Entities.Delete.DeleteTodoList()
            {
                Id = filter.Id,
                Name = filter.Name
            });
            return "Done";
        }
    }
}
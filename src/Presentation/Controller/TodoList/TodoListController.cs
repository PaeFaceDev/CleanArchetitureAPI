using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Response;
using Application.UseCases.TodoList;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller.TodoList
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "TodoList")]
    public class TodoListController : ControllerBase
    {
        private readonly TodoListUseCase _todoListUseCase1;
        public TodoListController(TodoListUseCase todoListUseCase)
        {
            _todoListUseCase1 = todoListUseCase;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTodoListAsync()
        {
            var resulte = await _todoListUseCase1.GetTodoListDtosAsync();
            return Ok(new ApiResponse<TodoListDtos>()
            {
                Data = resulte
            });
        }
    }
}
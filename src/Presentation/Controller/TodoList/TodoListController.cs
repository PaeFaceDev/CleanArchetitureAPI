using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Dtos.TodoList.Insert;
using Application.Dtos.TodoList.Update;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] TodoListInsertDtos argument)
        {
            await _todoListUseCase1.CreateTodoList(argument);
            return Ok(new ApiResponse<List<string>>
            {
                Code = "200",
                Messages = "Create Data Success",
                Data = [],
                Status = true
            });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateTodoList([FromQuery] TodoListUpdateDtos argument)
        {
            await _todoListUseCase1.UpdateTodoList(argument);
            return Ok(new ApiResponse<List<string>>
            {
                Code = "200",
                Messages = "Update Data Success",
                Data = [],
                Status = true
            });
        }
    }
}
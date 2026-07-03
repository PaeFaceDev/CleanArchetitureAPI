using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Dtos.TodoList.Insert;
using Application.Dtos.TodoList.Update;
using Application.Dtos.TodoList.Delete;
using Application.Response;
using Application.Features.TodoList.Commands;
using Application.Features.TodoList.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller.TodoList
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "TodoList")]
    public class TodoListController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TodoListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTodoListAsync")]
        public async Task<IActionResult> GetTodoListAsync()
        {
            var result = await _mediator.Send(new GetTodoListQuery());
            return Ok(new ApiResponse<TodoListDtos>()
            {
                Data = result
            });
        }

        [HttpPost("CreateTodoListAsync")]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] TodoListInsertDtos argument)
        {
            var command = CreateTodoListCommand.FromDto(argument);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<List<string>>
            {
                Code = "200",
                Messages = result,
                Data = [],
                Status = true
            });
        }

        [HttpPut("UpdateTodoList")]
        public async Task<IActionResult> UpdateTodoList([FromQuery] TodoListUpdateDtos argument)
        {
            var command = UpdateTodoListCommand.FromDto(argument);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<List<string>>
            {
                Code = "200",
                Messages = result,
                Data = [],
                Status = true
            });
        }
        [HttpDelete("DeleteTodoList")]
        public async Task<IActionResult> DeleteTodoList([FromQuery] DeleteTodoListDtos filter)
        {
            var command = DeleteTodoListCommand.FromDto(filter);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<List<string>>
            {
                Code = "200",
                Messages = result,
                Data = [],
                Status = true
            });
        }
    }
}
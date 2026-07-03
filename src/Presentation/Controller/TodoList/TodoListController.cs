using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos.TodoList.Insert;
using Application.Dtos.TodoList.Update;
using Application.Dtos.TodoList.Delete;
using Application.Dtos.TodoList;
using Application.Response;
using Application.Features.TodoList.Commands;
using Application.Features.TodoList.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Logging;

namespace Presentation.Controller.TodoList
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "TodoList")]
    public class TodoListController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(IMediator mediator, ILogger<TodoListController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetTodoListAsync")]
        public async Task<IActionResult> GetTodoListAsync()
        {
            try
            {
                _logger.LogInformation("GetTodoListAsync called");
                LoggingService.LogApiCall("GET", "/api/TodoList/GetTodoListAsync");
                
                var result = await _mediator.Send(new GetTodoListQuery());
                
                // Don't log the entire result object as it's very slow for large datasets
                // LoggingService.LogQueryExecution("GetTodoListQuery", result);
                _logger.LogInformation("GetTodoListAsync completed successfully. Retrieved {Count} items", result?.Count ?? 0);
                
                return Ok(new ApiResponse<List<TodoListDtos>>
                {
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodoListAsync");
                LoggingService.LogError(ex, "GetTodoListAsync failed");
                throw;
            }
        }

        [HttpPost("CreateTodoListAsync")]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] TodoListInsertDtos argument)
        {
            try
            {
                _logger.LogInformation("CreateTodoListAsync called with data: {@Argument}", argument);
                LoggingService.LogApiCall("POST", "/api/TodoList/CreateTodoListAsync");
                
                var command = CreateTodoListCommand.FromDto(argument);
                var result = await _mediator.Send(command);
                
                LoggingService.LogCommandEvent("CreateTodoListCommand", command);
                _logger.LogInformation("CreateTodoListAsync completed successfully");
                
                return Ok(new ApiResponse<List<string>>
                {
                    Code = "200",
                    Messages = result,
                    Data = [],
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTodoListAsync with data: {@Argument}", argument);
                LoggingService.LogError(ex, "CreateTodoListAsync failed");
                throw;
            }
        }

        [HttpPut("UpdateTodoList")]
        public async Task<IActionResult> UpdateTodoList([FromQuery] TodoListUpdateDtos argument)
        {
            try
            {
                _logger.LogInformation("UpdateTodoList called with data: {@Argument}", argument);
                LoggingService.LogApiCall("PUT", "/api/TodoList/UpdateTodoList");
                
                var command = UpdateTodoListCommand.FromDto(argument);
                var result = await _mediator.Send(command);
                
                LoggingService.LogCommandEvent("UpdateTodoListCommand", command);
                _logger.LogInformation("UpdateTodoList completed successfully");
                
                return Ok(new ApiResponse<List<string>>
                {
                    Code = "200",
                    Messages = result,
                    Data = [],
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTodoList with data: {@Argument}", argument);
                LoggingService.LogError(ex, "UpdateTodoList failed");
                throw;
            }
        }

        [HttpDelete("DeleteTodoList")]
        public async Task<IActionResult> DeleteTodoList([FromQuery] DeleteTodoListDtos filter)
        {
            try
            {
                _logger.LogInformation("DeleteTodoList called with data: {@Filter}", filter);
                LoggingService.LogApiCall("DELETE", "/api/TodoList/DeleteTodoList");
                
                var command = DeleteTodoListCommand.FromDto(filter);
                var result = await _mediator.Send(command);
                
                LoggingService.LogCommandEvent("DeleteTodoListCommand", command);
                _logger.LogInformation("DeleteTodoList completed successfully");
                
                return Ok(new ApiResponse<List<string>>
                {
                    Code = "200",
                    Messages = result,
                    Data = [],
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTodoList with data: {@Filter}", filter);
                LoggingService.LogError(ex, "DeleteTodoList failed");
                throw;
            }
        }
    }
}
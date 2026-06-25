using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.TodoList;
using Application.Dtos.TodoList.Insert;
using Application.UseCases.TodoList;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller.Sample
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Sample")]
    public class SampleApiController : ControllerBase
    {
        private readonly TodoListUseCase _todoListUseCase;
        public SampleApiController(TodoListUseCase todoListUseCase)
        {
            _todoListUseCase = todoListUseCase;
        }

        [HttpGet("[action]")]
        public async Task<List<TodoListDtos>> GetDataSampleAsync()
        {
            return await _todoListUseCase.GetTodoListDtosAsync();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateDataSamle([FromQuery] TodoListInsertDtos argument)
        {
            return Ok(await _todoListUseCase.CreateTodoList(argument));
        }

    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos.Employee;
using Application.Features.Employee.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Logging;

namespace Presentation.Controller.Employee
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetEmployeesByDateRange")]
        public async Task<IActionResult> GetEmployeesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation("GetEmployeesByDateRange called - StartDate: {StartDate}, EndDate: {EndDate}", startDate, endDate);
                LoggingService.LogApiCall("GET", "/api/Employee/GetEmployeesByDateRange");
                
                var query = new GetEmployeesByDateRangeQuery
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
                
                var result = await _mediator.Send(query);
                
                // Don't log the entire result object as it's very slow for large datasets
                // LoggingService.LogQueryExecution("GetEmployeesByDateRangeQuery", result);
                _logger.LogInformation("GetEmployeesByDateRange completed successfully. Retrieved {Count} employees", result?.Count ?? 0);
                
                return Ok(new ApiResponse<List<EmployeeDto>>
                {
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmployeesByDateRange with StartDate: {StartDate}, EndDate: {EndDate}", startDate, endDate);
                LoggingService.LogError(ex, "GetEmployeesByDateRange failed");
                throw;
            }
        }
    }
}
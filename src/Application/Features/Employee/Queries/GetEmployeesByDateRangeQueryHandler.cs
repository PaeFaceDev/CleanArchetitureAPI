using MediatR;
using Domain.Repositories;
using Application.Dtos.Employee;
using Microsoft.Extensions.Logging;

namespace Application.Features.Employee.Queries
{
    public class GetEmployeesByDateRangeQueryHandler : IRequestHandler<GetEmployeesByDateRangeQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetEmployeesByDateRangeQueryHandler> _logger;

        public GetEmployeesByDateRangeQueryHandler(IEmployeeRepository employeeRepository, ILogger<GetEmployeesByDateRangeQueryHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetEmployeesByDateRangeQueryHandler called - StartDate: {StartDate}, EndDate: {EndDate}", request.StartDate, request.EndDate);
                
                var employees = await _employeeRepository.GetEmployeesByDateRangeAsync(request.StartDate, request.EndDate);
                
                _logger.LogInformation("Retrieved {Count} employees", employees.Count);
                
                var result = employees.Select(e => new EmployeeDto
                {
                    Workday = e.Workday,
                    AccessDate = e.AccessDate,
                    AccessTime = e.AccessTime,
                    FullName = e.FullName,
                    Department = e.Department,
                    EmployeeID = e.EmployeeID,
                    NameTitle = e.NameTitle,
                    Shift = e.Shift,
                    Factory = e.Factory
                }).ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmployeesByDateRangeQueryHandler");
                throw;
            }
        }
    }
}
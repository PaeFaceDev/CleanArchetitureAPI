using MediatR;
using Application.Dtos.Employee;

namespace Application.Features.Employee.Queries
{
    public record GetEmployeesByDateRangeQuery : IRequest<List<EmployeeDto>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
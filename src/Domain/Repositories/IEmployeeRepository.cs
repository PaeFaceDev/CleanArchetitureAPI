using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Employee>> GetEmployeesByDateAsync(DateTime date);
        Task<List<Employee>> GetEmployeesByEmployeeIdAsync(string employeeId);
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
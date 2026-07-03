using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Employee
{
    public class EmployeeDto
    {
        public string? Workday { get; set; }
        public DateTime? AccessDate { get; set; }
        public TimeSpan? AccessTime { get; set; }
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? EmployeeID { get; set; }
        public string? NameTitle { get; set; }
        public string? Shift { get; set; }
        public string? Factory { get; set; }
    }
}
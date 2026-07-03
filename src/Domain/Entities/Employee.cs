namespace Domain.Entities
{
    public class Employee
    {
        public string Workday { get; set; } = string.Empty;
        public DateTime AccessDate { get; set; }
        public TimeSpan AccessTime { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public string NameTitle { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Factory { get; set; } = string.Empty;
    }
}
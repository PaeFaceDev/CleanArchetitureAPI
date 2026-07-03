using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Database.FaceScan;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly FaceScanDbContext _faceScanDbContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(FaceScanDbContext faceScanDbContext, IMemoryCache cache, ILogger<EmployeeRepository> logger)
        {
            _faceScanDbContext = faceScanDbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<Employee>> GetEmployeesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("GetEmployeesByDateRangeAsync called - StartDate: {StartDate}, EndDate: {EndDate}", startDate, endDate);
            
            // Try to get from cache - cache by individual days
            var allEmployees = new List<Employee>();
            var currentDate = startDate;
            
            while (currentDate <= endDate)
            {
                var dayCacheKey = $"employees_day_{currentDate:yyyyMMdd}";
                
                _logger.LogInformation("Checking cache for day: {Date}, Key: {CacheKey}", currentDate.ToString("yyyy-MM-dd"), dayCacheKey);
                
                if (_cache.TryGetValue(dayCacheKey, out List<Employee>? dayEmployees))
                {
                    if (dayEmployees != null)
                    {
                        _logger.LogInformation("Cache HIT for day: {Date}, Count: {Count}", currentDate.ToString("yyyy-MM-dd"), dayEmployees.Count);
                        allEmployees.AddRange(dayEmployees);
                    }
                }
                else
                {
                    // If not in cache, query for this specific day
                    var dateStr = currentDate.ToString("yyyy-MM-dd");
                    
                    var connection = _faceScanDbContext.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                            SELECT 
                                t.EMPLOYEEID,
                                t.ACCESSDATE,
                                t.ACCESSTIME,
                                t.PERSONGROUP,
                                t.HTCPERSONID,
                                t.FACTORY,
                                t.PERSONNAME,
                                t.TITLE,
                                t.SHIFT
                            FROM public.tbscantime as t
                            WHERE t.ACCESSDATE >= @date
                                AND t.ACCESSTIME >= '00:00:00'
                                AND t.ACCESSTIME <= '23:59:59'
                                AND t.EMPLOYEEID not like 'V%'
                            ORDER BY t.ACCESSTIME desc";
                        
                        var dateParam = command.CreateParameter();
                        dateParam.ParameterName = "@date";
                        dateParam.Value = dateStr;
                        command.Parameters.Add(dateParam);
                        
                        using var reader = await command.ExecuteReaderAsync();
                        dayEmployees = new List<Employee>();
                        
                        while (await reader.ReadAsync())
                        {
                            dayEmployees.Add(new Employee
                            {
                                Workday = reader["EMPLOYEEID"].ToString() ?? string.Empty,
                                AccessDate = DateTime.Parse(reader["ACCESSDATE"].ToString() ?? "1900-01-01"),
                                AccessTime = TimeSpan.Parse(reader["ACCESSTIME"].ToString() ?? "00:00:00"),
                                Department = reader["PERSONGROUP"].ToString() ?? string.Empty,
                                EmployeeID = reader["HTCPERSONID"].ToString() ?? string.Empty,
                                Factory = reader["FACTORY"].ToString() ?? string.Empty,
                                FullName = reader["PERSONNAME"].ToString() ?? string.Empty,
                                NameTitle = reader["TITLE"].ToString() ?? string.Empty,
                                Shift = reader["SHIFT"].ToString() ?? string.Empty
                            });
                        }
                        
                        // Cache this day's data for 1 hour
                        _cache.Set(dayCacheKey, dayEmployees, TimeSpan.FromHours(1));
                        _logger.LogInformation("Cached data for day: {Date}, Count: {Count}", currentDate.ToString("yyyy-MM-dd"), dayEmployees.Count);
                        allEmployees.AddRange(dayEmployees);
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
                
                currentDate = currentDate.AddDays(1);
            }
            
            _logger.LogInformation("GetEmployeesByDateRangeAsync completed - Total employees: {Count}", allEmployees.Count);
            return allEmployees;
        }

        public async Task<List<Employee>> GetEmployeesByDateAsync(DateTime date)
        {
            var cacheKey = $"employees_day_{date:yyyyMMdd}";
            
            _logger.LogInformation("Checking cache for key: {CacheKey}", cacheKey);
            
            if (_cache.TryGetValue(cacheKey, out List<Employee>? cachedEmployees))
            {
                if (cachedEmployees != null)
                {
                    _logger.LogInformation("Cache HIT for key: {CacheKey}, returning {Count} employees", cacheKey, cachedEmployees.Count);
                    return cachedEmployees;
                }
            }
            
            _logger.LogInformation("Cache MISS for key: {CacheKey}, querying database", cacheKey);

            var dateStr = date.ToString("yyyy-MM-dd");
            
            var connection = _faceScanDbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        t.EMPLOYEEID,
                        t.ACCESSDATE,
                        t.ACCESSTIME,
                        t.PERSONGROUP,
                        t.HTCPERSONID,
                        t.FACTORY,
                        t.PERSONNAME,
                        t.TITLE,
                        t.SHIFT
                    FROM public.tbscantime as t
                    WHERE t.ACCESSDATE >= @date
                        AND t.ACCESSTIME >= '00:00:00'
                        AND t.ACCESSTIME <= '23:59:59'
                        AND t.EMPLOYEEID not like 'V%'
                    ORDER BY t.ACCESSTIME desc";
                
                var dateParam = command.CreateParameter();
                dateParam.ParameterName = "@date";
                dateParam.Value = dateStr;
                command.Parameters.Add(dateParam);
                
                using var reader = await command.ExecuteReaderAsync();
                var employees = new List<Employee>();
                
                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee
                    {
                        Workday = reader["EMPLOYEEID"].ToString() ?? string.Empty,
                        AccessDate = DateTime.Parse(reader["ACCESSDATE"].ToString() ?? "1900-01-01"),
                        AccessTime = TimeSpan.Parse(reader["ACCESSTIME"].ToString() ?? "00:00:00"),
                        Department = reader["PERSONGROUP"].ToString() ?? string.Empty,
                        EmployeeID = reader["HTCPERSONID"].ToString() ?? string.Empty,
                        Factory = reader["FACTORY"].ToString() ?? string.Empty,
                        FullName = reader["PERSONNAME"].ToString() ?? string.Empty,
                        NameTitle = reader["TITLE"].ToString() ?? string.Empty,
                        Shift = reader["SHIFT"].ToString() ?? string.Empty
                    });
                }
                
                _logger.LogInformation("Query returned {Count} employees, caching for 1 hour", employees.Count);
                
                // Cache for 1 hour instead of 30 minutes
                _cache.Set(cacheKey, employees, TimeSpan.FromHours(1));
                return employees;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Employee>> GetEmployeesByEmployeeIdAsync(string employeeId)
        {
            var cacheKey = $"employee_{employeeId}";
            
            if (_cache.TryGetValue(cacheKey, out List<Employee> cachedEmployees))
            {
                return cachedEmployees;
            }

            var connection = _faceScanDbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        t.EMPLOYEEID,
                        t.ACCESSDATE,
                        t.ACCESSTIME,
                        t.PERSONGROUP,
                        t.HTCPERSONID,
                        t.FACTORY,
                        t.PERSONNAME,
                        t.TITLE,
                        t.SHIFT
                    FROM public.tbscantime as t
                    WHERE t.HTCPERSONID = @employeeId
                        AND t.EMPLOYEEID not like 'V%'
                    ORDER BY t.ACCESSDATE desc, t.ACCESSTIME desc";
                
                var employeeIdParam = command.CreateParameter();
                employeeIdParam.ParameterName = "@employeeId";
                employeeIdParam.Value = employeeId;
                command.Parameters.Add(employeeIdParam);
                
                using var reader = await command.ExecuteReaderAsync();
                var employees = new List<Employee>();
                
                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee
                    {
                        Workday = reader["EMPLOYEEID"].ToString() ?? string.Empty,
                        AccessDate = DateTime.Parse(reader["ACCESSDATE"].ToString() ?? "1900-01-01"),
                        AccessTime = TimeSpan.Parse(reader["ACCESSTIME"].ToString() ?? "00:00:00"),
                        Department = reader["PERSONGROUP"].ToString() ?? string.Empty,
                        EmployeeID = reader["HTCPERSONID"].ToString() ?? string.Empty,
                        Factory = reader["FACTORY"].ToString() ?? string.Empty,
                        FullName = reader["PERSONNAME"].ToString() ?? string.Empty,
                        NameTitle = reader["TITLE"].ToString() ?? string.Empty,
                        Shift = reader["SHIFT"].ToString() ?? string.Empty
                    });
                }
                
                _cache.Set(cacheKey, employees, TimeSpan.FromMinutes(30));
                return employees;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var cacheKey = "all_employees";
            
            if (_cache.TryGetValue(cacheKey, out List<Employee> cachedEmployees))
            {
                return cachedEmployees;
            }

            var connection = _faceScanDbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        t.EMPLOYEEID,
                        t.ACCESSDATE,
                        t.ACCESSTIME,
                        t.PERSONGROUP,
                        t.HTCPERSONID,
                        t.FACTORY,
                        t.PERSONNAME,
                        t.TITLE,
                        t.SHIFT
                    FROM public.tbscantime as t
                    WHERE t.EMPLOYEEID not like 'V%'
                    ORDER BY t.ACCESSDATE desc, t.ACCESSTIME desc";
                
                using var reader = await command.ExecuteReaderAsync();
                var employees = new List<Employee>();
                
                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee
                    {
                        Workday = reader["EMPLOYEEID"].ToString() ?? string.Empty,
                        AccessDate = DateTime.Parse(reader["ACCESSDATE"].ToString() ?? "1900-01-01"),
                        AccessTime = TimeSpan.Parse(reader["ACCESSTIME"].ToString() ?? "00:00:00"),
                        Department = reader["PERSONGROUP"].ToString() ?? string.Empty,
                        EmployeeID = reader["HTCPERSONID"].ToString() ?? string.Empty,
                        Factory = reader["FACTORY"].ToString() ?? string.Empty,
                        FullName = reader["PERSONNAME"].ToString() ?? string.Empty,
                        NameTitle = reader["TITLE"].ToString() ?? string.Empty,
                        Shift = reader["SHIFT"].ToString() ?? string.Empty
                    });
                }
                
                _cache.Set(cacheKey, employees, TimeSpan.FromMinutes(30));
                return employees;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
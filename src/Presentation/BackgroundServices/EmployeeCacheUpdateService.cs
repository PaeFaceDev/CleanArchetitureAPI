using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.BackgroundServices
{
    public class EmployeeCacheUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmployeeCacheUpdateService> _logger;

        public EmployeeCacheUpdateService(IServiceScopeFactory scopeFactory, ILogger<EmployeeCacheUpdateService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmployeeCacheUpdateService started");
            
            // Run immediately on startup
            await UpdateEmployeeCacheAsync();
            
            // Schedule to run every 30 minutes
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Waiting 30 minutes before next cache update");
                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                    
                    _logger.LogInformation("Starting scheduled cache update");
                    await UpdateEmployeeCacheAsync();
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("EmployeeCacheUpdateService stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in EmployeeCacheUpdateService execution");
                    // Continue running despite errors
                }
            }
            
            _logger.LogInformation("EmployeeCacheUpdateService stopped");
        }

        private async Task UpdateEmployeeCacheAsync()
        {
            try
            {
                _logger.LogInformation("Updating employee cache");
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                    
                    // Cache data for today and next 7 days
                    var today = DateTime.Today;
                    var daysToCache = 7;
                    
                    for (int i = 0; i < daysToCache; i++)
                    {
                        var targetDate = today.AddDays(i);
                        try
                        {
                            var employees = await employeeRepository.GetEmployeesByDateAsync(targetDate);
                            _logger.LogInformation("Cached data for {Date}. Total employees: {Count}", 
                                targetDate.ToString("yyyy-MM-dd"), employees.Count);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to cache data for {Date}", targetDate.ToString("yyyy-MM-dd"));
                        }
                    }
                    
                    _logger.LogInformation("Employee cache update completed for {Days} days", daysToCache);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee cache - Database connection may not be configured yet");
                // Don't throw - let the service continue running for the next scheduled update
            }
        }
    }
}
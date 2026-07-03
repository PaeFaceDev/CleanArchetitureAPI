using Domain.Repositories;
using Domain.Repositories.TodoList;
using Infrastructure.Persistence.Database.FaceScan;
using Infrastructure.Persistence.Database.TodoList;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.TodoList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddTodoListDatabase(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("MysqlTodoList");
            services.AddDbContext<TodoListDbContext>(
                options =>
                {
                    options.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString)
                    );
                }
            );
            services.AddScoped<ITodoListRepositories, TodoListRepositories>();
            return services;
        }
        
        public static IServiceCollection AddFaceScanDatabase(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("FaceScan");
            services.AddDbContext<FaceScanDbContext>(
                options =>
                {
                    options.UseNpgsql(connectionString);
                }
            );
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
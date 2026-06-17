using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositories.TodoList;
using Infrastructure.Persistence.Repositories.TodoList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Database.TodoListMysql
{
    public static class DependencyInjection
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
            services.AddScoped<ITodoListRepositories, GetTodoListRepositories>();
            return services;

        }
    }
}
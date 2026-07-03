using Infrastructure;
using Microsoft.OpenApi;
using Presentation.SignalRHubs;
using Presentation.DomainEventHandlers;
using Presentation.Middleware;
using Presentation.BackgroundServices;
using Domain.Common;
using MediatR;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Hosting;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "CleanArchitectureAPI")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/log-.txt", 
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

try
{
    Log.Information("Starting CleanArchitectureAPI application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddSignalR();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Features.TodoList.Commands.CreateTodoListCommand).Assembly));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Features.Employee.Queries.GetEmployeesByDateRangeQuery).Assembly));
    builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

    builder.Services.AddMemoryCache();
    
    // Add Serilog
    builder.Host.UseSerilog();
    
    // Add Background Service
    builder.Services.AddHostedService<Presentation.BackgroundServices.EmployeeCacheUpdateService>();
    
    // Configure background service to not stop host on error
    builder.Services.Configure<HostOptions>(options =>
    {
        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    });
    
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("Sample", new OpenApiInfo { Version = "v1", Title = "Sample API" });
        c.SwaggerDoc("TodoList", new OpenApiInfo { Version = "v1", Title = "TodoList API" });
        c.SwaggerDoc("Employee", new OpenApiInfo { Version = "v1", Title = "Employee API" });
        c.DocInclusionPredicate((groupName, apiDesc) => apiDesc.GroupName == groupName);
    });
    
    var app = builder.Build();

    // Add global exception middleware
    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Sample/swagger.json", "Sample API");
        c.SwaggerEndpoint("/swagger/TodoList/swagger.json", "TodoList API");
        c.SwaggerEndpoint("/swagger/Employee/swagger.json", "Employee API");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
    
    // app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.MapControllers();
    app.MapHub<TodoListHub>("/todolisthub");
    app.Map("/", () => Results.Redirect("/swagger/index.html"));

    Log.Information("Application started successfully");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}
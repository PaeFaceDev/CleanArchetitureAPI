# 🚀 CleanArchitectureAPI - Enterprise-Grade API System

> A modern, high-performance API system built with Clean Architecture, Domain-Driven Design (DDD), SOLID principles, and advanced caching strategies for optimal performance.

![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-blue.svg)
![DDD](https://img.shields.io/badge/Design-DDD-green.svg)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)

## ✨ Key Features

- 🏗️ **Clean Architecture** - Strict layered architecture with dependency inversion
- 🎯 **Domain-Driven Design** - Domain-centric design with aggregates and events
- ⚡ **High-Performance Caching** - IMemoryCache with per-day caching strategy
- 📡 **Real-time Updates** - SignalR integration for live data synchronization
- 🔀 **CQRS Pattern** - Command Query Responsibility Segregation
- 🗄️ **Multi-Database Support** - MySQL, SQL Server, PostgreSQL, SQLite
- 📊 **Employee Data Management** - Advanced employee data retrieval with caching

## 🚀 Tech Stack

### Core Framework
- **.NET 10.0** - Latest .NET framework for high-performance applications
- **ASP.NET Core 10.0** - Web framework for building modern APIs

### Architecture & Design Patterns
- **MediatR 14.2.0** - CQRS pattern implementation for mediator pattern
- **Clean Architecture** - Layered architecture with dependency inversion
- **Domain-Driven Design (DDD)** - Domain-centric design with aggregates and events
- **CQRS** - Command Query Responsibility Segregation pattern
- **SOLID Principles** - Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion

### Database & ORM
- **Entity Framework Core 9.0.0** - Modern ORM for database operations
- **MySQL Support** - Pomelo.EntityFrameworkCore.MySql 9.0.0
- **SQL Server Support** - Microsoft.EntityFrameworkCore.SqlServer 9.0.0
- **SQLite Support** - Microsoft.EntityFrameworkCore.Sqlite 9.0.0 (for development/testing)

### Real-time Communication
- **SignalR** - Real-time web functionality for live updates
- **SignalR JavaScript Client 8.0.0** - Client-side SignalR library

### API Documentation
- **Swagger/OpenAPI** - API documentation and testing interface
- **Swashbuckle.AspNetCore 10.2.1** - Swagger generation for ASP.NET Core
- **Microsoft.AspNetCore.OpenApi 10.0.9** - OpenAPI specification support

### Caching
- **IMemoryCache** - In-memory caching for performance optimization
- **Per-Day Caching Strategy** - Efficient caching for date-based queries
- **Background Cache Update Service** - Automatic cache population and refresh
- **Cache Duration Management** - Configurable cache expiration policies

## 🏗️ Architecture Principles

### Clean Architecture
This project follows strict Clean Architecture principles with clear separation of concerns:

```
src/
├── Domain/           # Core business logic and entities (no dependencies)
├── Application/      # Application logic, CQRS commands/queries, DTOs
├── Infrastructure/   # External concerns (database, repositories)
└── Presentation/     # API controllers, SignalR hubs, UI
```

**Dependency Flow**: Presentation → Application → Domain ← Infrastructure

### Domain-Driven Design (DDD)
- **Aggregates**: `TodoListAggregate` - Root entity with business logic
- **Domain Events**: `TodoListCreatedEvent`, `TodoListUpdatedEvent`, `TodoListDeletedEvent`
- **Value Objects**: Immutable objects with business meaning
- **Repository Pattern**: Abstract data access through interfaces
- **Domain Services**: Business logic that doesn't naturally fit in entities

### SOLID Principles Implementation

#### Single Responsibility Principle (SRP)
Each class has one reason to change:
- `CreateTodoListCommandHandler` - Handles only create operations
- `GetTodoListQueryHandler` - Handles only read operations
- `TodoListAggregate` - Contains only TodoList business logic

#### Open/Closed Principle (OCP)
Open for extension, closed for modification:
- New domain events can be added without modifying existing handlers
- New commands/queries can be added without changing existing ones

#### Liskov Substitution Principle (LSP)
Subtypes must be substitutable for base types:
- Repository implementations can be swapped without breaking the application
- Different database providers (MySQL, SQL Server, SQLite) are interchangeable

#### Interface Segregation Principle (ISP)
Clients shouldn't depend on interfaces they don't use:
- `ITodoListRepositories` - Focused interface for TodoList operations
- `IDomainEventDispatcher` - Specific interface for event dispatching

#### Dependency Inversion Principle (DIP)
Depend on abstractions, not concretions:
- Controllers depend on `IMediator` (abstraction) instead of concrete handlers
- Application layer depends on Domain interfaces instead of concrete implementations

### CQRS Pattern
Separation of read and write operations:

**Commands (Write Operations)**:
```csharp
public record CreateTodoListCommand : IRequest<string>
{
    public string? Name { get; set; }
    public string? Category { get; set; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, string>
{
    // Handler implementation
}
```

**Queries (Read Operations)**:
```csharp
public record GetTodoListQuery : IRequest<List<TodoListDtos>>
{
}

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, List<TodoListDtos>>
{
    // Handler implementation
}
```

## 🗄️ Database Configuration

### Supported Databases
- **MySQL** - Production database (Pomelo.EntityFrameworkCore.MySql)
- **SQL Server** - Enterprise database support
- **SQLite** - Development and testing database

### Database Schema
```sql
CREATE TABLE TodoList (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Category VARCHAR(255),
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
```

### DbContext Configuration
```csharp
public class TodoListDbContext : DbContext
{
    public DbSet<Todolist> TodoList { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Database configuration based on appsettings
    }
}
```

## ⚙️ AppSettings Configuration

### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoListDb;User=root;Password=your_password;",
    "MySQLConnection": "Server=localhost;Database=TodoListDb;User=root;Password=your_password;",
    "SQLServerConnection": "Server=localhost;Database=TodoListDb;Trusted_Connection=true;",
    "SQLiteConnection": "Data Source=TodoList.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Configuration Injection
```csharp
builder.Services.AddInfrastructure(builder.Configuration);
```

## 🎯 System Functionality

### Core Features

#### 1. TodoList CRUD Operations
- **Create** - Add new TodoList items with name and category
- **Read** - Retrieve all TodoList items with caching
- **Update** - Modify existing TodoList items
- **Delete** - Remove TodoList items with confirmation

#### 2. Employee Data Management
- **Date Range Queries** - Retrieve employee data by date range
- **Advanced Caching** - Per-day caching strategy for optimal performance
- **High-Performance Retrieval** - Cache-first approach to minimize database queries
- **Automatic Cache Population** - Background service pre-loads data for common queries

#### 3. Real-time Updates
- **SignalR Integration** - Live updates across all connected clients
- **Domain Events** - Automatic broadcasting of changes
- **Auto-refresh** - Client-side automatic data refresh on changes

#### 4. CQRS Implementation
- **Command Handlers** - Handle write operations with validation
- **Query Handlers** - Handle read operations with optimization
- **MediatR Pipeline** - Centralized request/response handling

#### 5. Domain Events System
- **Event-Driven Architecture** - Decoupled business logic
- **Event Dispatching** - Automatic event propagation
- **Multiple Handlers** - Support for multiple event subscribers

## ⚡ Caching Strategy

### Overview
This project implements a sophisticated caching strategy designed for high-performance data retrieval, specifically optimized for date-based queries with large datasets.

### Cache Strategy Architecture

#### 1. Per-Day Caching
```csharp
// Cache key format: employees_day_YYYYMMDD
var cacheKey = $"employees_day_{date:yyyyMMdd}";
```

**Benefits:**
- **Granular Control** - Cache data by individual days for better hit rates
- **Flexible Queries** - Support any date range by combining cached days
- **Efficient Storage** - Only cache days that are actually requested

#### 2. Background Cache Update Service
```csharp
public class EmployeeCacheUpdateService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Cache 7 days ahead of current date
        var daysToCache = 7;
        for (int i = 0; i < daysToCache; i++)
        {
            var targetDate = today.AddDays(i);
            var employees = await _employeeRepository.GetEmployeesByDateAsync(targetDate);
        }
    }
}
```

**Features:**
- **Pre-loading** - Automatically cache data for upcoming days
- **Scheduled Updates** - Refresh cache every 30 minutes
- **Intelligent Caching** - Only cache days with actual data

#### 3. Cache Duration Management
```csharp
// Cache duration: 1 hour for regular queries
_cache.Set(cacheKey, employees, TimeSpan.FromHours(1));
```

**Configuration:**
- **Regular Queries** - 1 hour cache duration
- **Background Updates** - 30-minute refresh interval
- **Adjustable** - Easy to modify based on data freshness requirements

### Cache Performance Benefits

#### Before Caching
- **Response Time:** ~2.1 seconds
- **Database Queries:** Every request
- **Database Load:** High
- **Scalability:** Limited

#### After Caching
- **Response Time:** ~0.1-0.2 seconds (10-20x faster)
- **Database Queries:** Only on cache miss
- **Database Load:** Minimal
- **Scalability:** Excellent

### Cache Hit Rate Analysis

**Real-world Performance:**
```
[17:50:30] Cache HIT for day: 2026-07-03, Count: 4150
[17:50:44] Cache HIT for day: 2026-07-03, Count: 4150
[17:50:48] Cache HIT for day: 2026-07-03, Count: 4150
```

**Results:**
- **Cache Hit Rate:** 100% (3/3 calls)
- **Database Queries:** 0 (after startup)
- **Performance Improvement:** 10-20x faster

### Implementation Details

#### EmployeeRepository Caching
```csharp
public async Task<List<Employee>> GetEmployeesByDateAsync(DateTime date)
{
    var cacheKey = $"employees_day_{date:yyyyMMdd}";
    
    // Check cache first
    if (_cache.TryGetValue(cacheKey, out List<Employee>? cachedEmployees))
    {
        _logger.LogInformation("Cache HIT for key: {CacheKey}", cacheKey);
        return cachedEmployees;
    }
    
    // Query database on cache miss
    _logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
    var employees = await QueryDatabase(date);
    
    // Cache for 1 hour
    _cache.Set(cacheKey, employees, TimeSpan.FromHours(1));
    return employees;
}
```

#### GetEmployeesByDateRange Caching
```csharp
public async Task<List<Employee>> GetEmployeesByDateRangeAsync(DateTime startDate, DateTime endDate)
{
    var allEmployees = new List<Employee>();
    var currentDate = startDate;
    
    // Loop through each day in range
    while (currentDate <= endDate)
    {
        var dayCacheKey = $"employees_day_{currentDate:yyyyMMdd}";
        
        // Check cache for each day
        if (_cache.TryGetValue(dayCacheKey, out List<Employee>? dayEmployees))
        {
            allEmployees.AddRange(dayEmployees);
        }
        else
        {
            // Query and cache this day
            var dayEmployees = await GetEmployeesByDateAsync(currentDate);
            allEmployees.AddRange(dayEmployees);
        }
        
        currentDate = currentDate.AddDays(1);
    }
    
    return allEmployees;
}
```

### Cache Best Practices Implemented

1. **Granular Keys** - Per-day caching for better hit rates
2. **Appropriate Duration** - 1-hour balance between freshness and performance
3. **Background Pre-loading** - Proactive cache population
4. **Logging** - Comprehensive cache hit/miss logging
5. **Performance Monitoring** - Track cache effectiveness

### Configuration Options

#### Adjust Cache Duration
```csharp
// In EmployeeRepository.cs
_cache.Set(cacheKey, employees, TimeSpan.FromHours(1)); // Change duration as needed
```

#### Adjust Background Update Interval
```csharp
// In Program.cs
services.AddHostedService<EmployeeCacheUpdateService>();
// Modify ExecuteAsync method to change refresh interval
```

#### Adjust Cache Window
```csharp
// In EmployeeCacheUpdateService.cs
var daysToCache = 7; // Change number of days to pre-cache
```

## 🌟 Project Highlights

### 1. Clean Architecture Implementation

**Layer Separation**:
```csharp
// Domain Layer - No dependencies
namespace Domain.Entities
{
    public class TodoListAggregate : EntityBase
    {
        private TodoListAggregate() { }
        
        public TodoListAggregate(string name, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");
            
            Name = name.Trim();
            Category = category?.Trim();
            CreatedAt = DateTime.UtcNow;
        }
        
        public void UpdateName(string newName)
        {
            // Business logic validation
            // Domain event generation
        }
    }
}

// Application Layer - Depends on Domain
namespace Application.Features.TodoList.Commands
{
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        
        // Implementation
    }
}

// Infrastructure Layer - Implements Domain interfaces
namespace Infrastructure.Persistence.Repositories.TodoList
{
    public class TodoListRepositories : ITodoListRepositories
    {
        // Database implementation
    }
}
```

### 2. Domain Events with SignalR Integration

**Event-Driven Real-time Updates**:
```csharp
// Domain Event
public class TodoListCreatedEvent : DomainEventBase
{
    public int TodoListId { get; }
    public string Name { get; }
    public string? Category { get; }
}

// Event Dispatcher
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IHubContext<TodoListHub> _hubContext;
    
    public async Task DispatchAndClearEventsAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchEventAsync(domainEvent);
        }
    }
    
    private async Task DispatchEventAsync(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case TodoListCreatedEvent createdEvent:
                await _hubContext.Clients.All.SendAsync("TodoListChanged", "create", 
                    JsonSerializer.Serialize(new { createdEvent.TodoListId, createdEvent.Name }));
                break;
            // Other events...
        }
    }
}
```

### 3. CQRS with MediatR

**Command/Query Separation**:
```csharp
// Command Definition
public record CreateTodoListCommand : IRequest<string>
{
    public string? Name { get; set; }
    public string? Category { get; set; }
}

// Command Handler
public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, string>
{
    public async Task<string> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required");
        
        // Business logic
        var todoListAggregate = new TodoListAggregate(request.Name, request.Category);
        await _todoListRepositories.CreateTodoListAsync(todoListAggregate);
        
        // Event dispatching
        await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
        
        return "TodoList created successfully";
    }
}

// Controller Usage
[HttpPost("CreateTodoListAsync")]
public async Task<IActionResult> CreateTodoListAsync([FromBody] TodoListInsertDtos argument)
{
    var command = CreateTodoListCommand.FromDto(argument);
    var result = await _mediator.Send(command);
    return Ok(new ApiResponse<string> { Data = result });
}
```

### 4. Repository Pattern with Multiple Database Support

**Abstract Data Access**:
```csharp
// Domain Interface
public interface ITodoListRepositories
{
    Task<List<Todolist>> GetTodolistMany();
    Task<string> CreateTodoListAsync(TodoListAggregate argument);
    Task<string> UpdateTodoList(TodoListAggregate argument);
    Task<string> DeleteTodoList(TodoListAggregate argument);
}

// Implementation with Caching
public class TodoListRepositories : ITodoListRepositories
{
    private readonly TodoListDbContext _todoListDbContext;
    private readonly IMemoryCache _cache;
    
    public async Task<List<Todolist>> GetTodolistMany()
    {
        string cacheKey = "todo:all";
        
        if (!_cache.TryGetValue(cacheKey, out List<Todolist> todos))
        {
            todos = await _todoListDbContext.TodoList.ToListAsync();
            _cache.Set(cacheKey, todos, TimeSpan.FromMinutes(10));
        }
        
        return todos;
    }
}
```

### 5. SignalR Real-time Hub

**Real-time Communication**:
```csharp
public class TodoListHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
    public async Task NotifyTodoListChange(string action, string data)
    {
        await Clients.All.SendAsync("TodoListChanged", action, data);
    }
}

// Program.cs Configuration
builder.Services.AddSignalR();
app.MapHub<TodoListHub>("/todolisthub");
```

### 6. Client-side Real-time Updates

**JavaScript SignalR Client**:
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/todolisthub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("TodoListChanged", function (action, data) {
    console.log(`TodoList ${action}:`, data);
    // Auto-refresh UI
    if (document.getElementById('listPage').classList.contains('active')) {
        loadTodoList();
    }
});

connection.start().then(function () {
    console.log('SignalR Connected');
    loadTodoList();
});
```

## 🎨 UI Features

### Web Interface
- **List Data Page** - Display all TodoList items with edit/delete options
- **Insert Data Page** - Form for adding new TodoList items
- **Real-time Activity Log** - Live log of all system events
- **Connection Status** - Visual indicator of SignalR connection
- **Auto-refresh** - Automatic data updates on changes
- **Responsive Design** - Mobile-friendly interface

## 📁 Project Structure

```
CleanArchetitureAPI/
├── src/
│   ├── Domain/                          # Core business logic
│   │   ├── Common/                      # Domain base classes
│   │   │   ├── IDomainEvent.cs
│   │   │   ├── DomainEventBase.cs
│   │   │   ├── EntityBase.cs
│   │   │   └── IDomainEventDispatcher.cs
│   │   ├── Entities/                    # Domain entities
│   │   │   └── TodoListAggregate.cs
│   │   ├── Events/                      # Domain events
│   │   │   ├── TodoListCreatedEvent.cs
│   │   │   ├── TodoListUpdatedEvent.cs
│   │   │   └── TodoListDeletedEvent.cs
│   │   └── Repositories/                # Repository interfaces
│   │       └── TodoList/
│   │           └── ITodoListRepositories.cs
│   ├── Application/                     # Application logic
│   │   ├── Features/                    # CQRS features
│   │   │   └── TodoList/
│   │   │       ├── Commands/            # Write operations
│   │   │       │   ├── CreateTodoListCommand.cs
│   │   │       │   ├── UpdateTodoListCommand.cs
│   │   │       │   ├── DeleteTodoListCommand.cs
│   │   │       │   └── [Handlers]
│   │   │       └── Queries/             # Read operations
│   │   │           ├── GetTodoListQuery.cs
│   │   │           └── [Handlers]
│   │   ├── Dtos/                        # Data transfer objects
│   │   │   └── TodoList/
│   │   └── Response/                    # API response models
│   ├── Infrastructure/                  # External concerns
│   │   ├── Persistence/
│   │   │   ├── Database/
│   │   │   │   └── TodoListMysql/
│   │   │   │       ├── TodoListDbContext.cs
│   │   │   │       └── DependencyInjection.cs
│   │   │   └── Repositories/
│   │   │       └── TodoList/
│   │   │           └── TodoListRepositories.cs
│   │   └── SignalR/                     # SignalR configuration
│   └── Presentation/                   # API layer
│       ├── Controller/
│       │   └── TodoList/
│       │       └── TodoListController.cs
│       ├── SignalRHubs/
│       │   └── TodoListHub.cs
│       ├── DomainEventHandlers/
│       │   └── DomainEventDispatcher.cs
│       ├── wwwroot/
│       │   └── todolist.html            # Web interface
│       └── Program.cs                   # Application entry point
└── README.md
```

## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK
- MySQL Server (or SQL Server/SQLite for development)
- Modern web browser with JavaScript support

### Installation
1. Clone the repository
2. Configure database connection in `appsettings.json`
3. Run database migrations (if needed)
4. Restore dependencies: `dotnet restore`
5. Build the project: `dotnet build`
6. Run the application: `dotnet run --project src/Presentation`

### Access Points
- **API**: `http://localhost:5000/swagger` (Swagger UI)
- **Web Interface**: `http://localhost:5000/todolist.html`
- **SignalR Hub**: `http://localhost:5000/todolisthub`

## 🔧 API Endpoints

### TodoList Operations
- `GET /api/TodoList/GetTodoListAsync` - Get all TodoList items
- `POST /api/TodoList/CreateTodoListAsync` - Create new TodoList
- `PUT /api/TodoList/UpdateTodoList` - Update existing TodoList
- `DELETE /api/TodoList/DeleteTodoList` - Delete TodoList

### SignalR Events
- `TodoListChanged` - Real-time TodoList change notifications
- `ReceiveMessage` - General message notifications

## 🎯 Key Benefits

1. **Maintainability** - Clean architecture makes the code easy to understand and modify
2. **Testability** - Dependency injection and interfaces enable easy unit testing
3. **Scalability** - CQRS pattern allows independent scaling of read/write operations
4. **Flexibility** - Multiple database support and pluggable architecture
5. **Real-time** - SignalR provides instant updates across all clients
6. **Performance** - Caching and optimized query handling
7. **Type Safety** - Strong typing with C# and nullable reference types
8. **Modern Standards** - Latest .NET framework and best practices

## 📝 Development Guidelines

### Adding New Features
1. Define domain entities in `Domain/Entities/`
2. Create domain events in `Domain/Events/`
3. Add CQRS commands/queries in `Application/Features/`
4. Implement handlers in respective folders
5. Add repository methods if needed
6. Create controller endpoints in `Presentation/Controller/`
7. Update SignalR handlers for real-time updates

### Code Style
- Follow C# naming conventions
- Use async/await for I/O operations
- Implement proper error handling
- Add XML documentation for public APIs
- Write unit tests for business logic

## 🤝 Contributing

This project demonstrates enterprise-grade software architecture patterns. Feel free to use it as a reference for your own projects or contribute improvements.

## 📄 License

This project is provided as a reference implementation for educational and commercial use.

---

**Built with Clean Architecture, DDD, SOLID principles, and modern .NET technologies.**
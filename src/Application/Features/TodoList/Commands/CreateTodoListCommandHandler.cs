using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Features.TodoList.Commands
{
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<CreateTodoListCommandHandler> _logger;

        public CreateTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher, ILogger<CreateTodoListCommandHandler> logger)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task<string> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("CreateTodoListCommandHandler started with data: {@Request}", request);
                
                if (request == null)
                {
                    _logger.LogError("CreateTodoListCommand received null request");
                    throw new ArgumentNullException(nameof(request));
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("CreateTodoListCommand received empty name");
                    throw new ArgumentException("Name is required", nameof(request.Name));
                }

                _logger.LogInformation("Creating TodoListAggregate with Name: {Name}, Category: {Category}", request.Name, request.Category);
                var todoListAggregate = new TodoListAggregate(request.Name, request.Category);
                
                _logger.LogInformation("Calling repository to create TodoList");
                await _todoListRepositories.CreateTodoListAsync(todoListAggregate);
                
                _logger.LogInformation("Dispatching domain events: {EventCount} events", todoListAggregate.DomainEvents.Count());
                await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
                
                _logger.LogInformation("CreateTodoListCommandHandler completed successfully");
                return "TodoList created successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTodoListCommandHandler");
                throw;
            }
        }
    }
}
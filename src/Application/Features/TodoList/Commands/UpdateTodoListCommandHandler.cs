using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Features.TodoList.Commands
{
    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<UpdateTodoListCommandHandler> _logger;

        public UpdateTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher, ILogger<UpdateTodoListCommandHandler> logger)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task<string> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("UpdateTodoListCommandHandler started with data: {@Request}", request);
                
                if (request == null)
                {
                    _logger.LogError("UpdateTodoListCommand received null request");
                    throw new ArgumentNullException(nameof(request));
                }

                if (request.Id <= 0)
                {
                    _logger.LogWarning("UpdateTodoListCommand received invalid Id: {Id}", request.Id);
                    throw new ArgumentException("Invalid Id", nameof(request.Id));
                }

                _logger.LogInformation("Creating TodoListAggregate for update. Id: {Id}, Name: {Name}", request.Id, request.Name);
                var todoListAggregate = new TodoListAggregate(request.Name ?? string.Empty);
                todoListAggregate.SetId(request.Id);
                
                _logger.LogInformation("Calling repository to update TodoList");
                await _todoListRepositories.UpdateTodoList(todoListAggregate);
                
                _logger.LogInformation("Dispatching domain events: {EventCount} events", todoListAggregate.DomainEvents.Count());
                await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
                
                _logger.LogInformation("UpdateTodoListCommandHandler completed successfully");
                return "TodoList updated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTodoListCommandHandler");
                throw;
            }
        }
    }
}
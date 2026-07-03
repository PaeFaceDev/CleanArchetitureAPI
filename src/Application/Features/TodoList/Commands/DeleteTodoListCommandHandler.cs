using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Features.TodoList.Commands
{
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<DeleteTodoListCommandHandler> _logger;

        public DeleteTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher, ILogger<DeleteTodoListCommandHandler> logger)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task<string> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("DeleteTodoListCommandHandler started with data: {@Request}", request);
                
                if (request == null)
                {
                    _logger.LogError("DeleteTodoListCommand received null request");
                    throw new ArgumentNullException(nameof(request));
                }

                if (request.Id <= 0)
                {
                    _logger.LogWarning("DeleteTodoListCommand received invalid Id: {Id}", request.Id);
                    throw new ArgumentException("Invalid Id", nameof(request.Id));
                }

                _logger.LogInformation("Creating TodoListAggregate for deletion. Id: {Id}, Name: {Name}", request.Id, request.Name);
                var todoListAggregate = new TodoListAggregate(request.Name ?? string.Empty, request.Category);
                todoListAggregate.SetId(request.Id);
                todoListAggregate.MarkAsDeleted();
                
                _logger.LogInformation("Calling repository to delete TodoList");
                await _todoListRepositories.DeleteTodoList(todoListAggregate);
                
                _logger.LogInformation("Dispatching domain events: {EventCount} events", todoListAggregate.DomainEvents.Count());
                await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
                
                _logger.LogInformation("DeleteTodoListCommandHandler completed successfully");
                return "TodoList deleted successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTodoListCommandHandler");
                throw;
            }
        }
    }
}
using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;

namespace Application.Features.TodoList.Commands
{
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public CreateTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<string> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required", nameof(request.Name));
            }

            var todoListAggregate = new TodoListAggregate(request.Name, request.Category);
            
            await _todoListRepositories.CreateTodoListAsync(todoListAggregate);
            
            await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
            
            return "TodoList created successfully";
        }
    }
}
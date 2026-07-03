using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;

namespace Application.Features.TodoList.Commands
{
    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public UpdateTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<string> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id <= 0)
            {
                throw new ArgumentException("Invalid Id", nameof(request.Id));
            }

            var todoListAggregate = new TodoListAggregate(request.Name ?? string.Empty);
            todoListAggregate.SetId(request.Id);
            
            await _todoListRepositories.UpdateTodoList(todoListAggregate);
            
            await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
            
            return "TodoList updated successfully";
        }
    }
}
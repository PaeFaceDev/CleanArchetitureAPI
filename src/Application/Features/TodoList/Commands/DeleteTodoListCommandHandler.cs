using MediatR;
using Domain.Repositories.TodoList;
using Domain.Entities;
using Domain.Common;

namespace Application.Features.TodoList.Commands
{
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, string>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DeleteTodoListCommandHandler(ITodoListRepositories todoListRepositories, IDomainEventDispatcher domainEventDispatcher)
        {
            _todoListRepositories = todoListRepositories;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<string> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id <= 0)
            {
                throw new ArgumentException("Invalid Id", nameof(request.Id));
            }

            var todoListAggregate = new TodoListAggregate(request.Name ?? string.Empty, request.Category);
            todoListAggregate.SetId(request.Id);
            todoListAggregate.MarkAsDeleted();
            
            await _todoListRepositories.DeleteTodoList(todoListAggregate);
            
            await _domainEventDispatcher.DispatchAndClearEventsAsync(todoListAggregate.DomainEvents);
            
            return "TodoList deleted successfully";
        }
    }
}
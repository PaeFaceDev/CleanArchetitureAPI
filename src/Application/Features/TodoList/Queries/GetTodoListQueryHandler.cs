using MediatR;
using Domain.Repositories.TodoList;
using Application.Dtos.TodoList;

namespace Application.Features.TodoList.Queries
{
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, List<TodoListDtos>>
    {
        private readonly ITodoListRepositories _todoListRepositories;

        public GetTodoListQueryHandler(ITodoListRepositories todoListRepositories)
        {
            _todoListRepositories = todoListRepositories;
        }

        public async Task<List<TodoListDtos>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            var entities = await _todoListRepositories.GetTodolistMany();
            
            return entities.Select(e => new TodoListDtos
            {
                Id = e.Id,
                Category = e.Category,
                Name = e.Name
            }).ToList();
        }
    }
}
using MediatR;
using Domain.Repositories.TodoList;
using Application.Dtos.TodoList;
using Microsoft.Extensions.Logging;

namespace Application.Features.TodoList.Queries
{
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, List<TodoListDtos>>
    {
        private readonly ITodoListRepositories _todoListRepositories;
        private readonly ILogger<GetTodoListQueryHandler> _logger;

        public GetTodoListQueryHandler(ITodoListRepositories todoListRepositories, ILogger<GetTodoListQueryHandler> logger)
        {
            _todoListRepositories = todoListRepositories;
            _logger = logger;
        }

        public async Task<List<TodoListDtos>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetTodoListQueryHandler started");
                
                _logger.LogInformation("Calling repository to get TodoList items");
                var entities = await _todoListRepositories.GetTodolistMany();
                
                _logger.LogInformation("Retrieved {Count} TodoList items from repository", entities?.Count ?? 0);
                
                var result = entities.Select(e => new TodoListDtos
                {
                    Id = e.Id,
                    Category = e.Category,
                    Name = e.Name
                }).ToList();
                
                _logger.LogInformation("GetTodoListQueryHandler completed successfully. Returning {Count} items", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodoListQueryHandler");
                throw;
            }
        }
    }
}
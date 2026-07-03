using MediatR;
using Application.Dtos.TodoList.Insert;

namespace Application.Features.TodoList.Commands
{
    public record CreateTodoListCommand : IRequest<string>
    {
        public string? Name { get; set; }
        public string? Category { get; set; }

        public static CreateTodoListCommand FromDto(TodoListInsertDtos dto)
        {
            return new CreateTodoListCommand
            {
                Name = dto.Name,
                Category = dto.Category
            };
        }
    }
}
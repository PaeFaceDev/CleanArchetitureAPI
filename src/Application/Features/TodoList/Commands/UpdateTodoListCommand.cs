using MediatR;
using Application.Dtos.TodoList.Update;

namespace Application.Features.TodoList.Commands
{
    public record UpdateTodoListCommand : IRequest<string>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public static UpdateTodoListCommand FromDto(TodoListUpdateDtos dto)
        {
            return new UpdateTodoListCommand
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
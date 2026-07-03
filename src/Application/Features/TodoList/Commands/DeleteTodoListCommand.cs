using MediatR;
using Application.Dtos.TodoList.Delete;

namespace Application.Features.TodoList.Commands
{
    public record DeleteTodoListCommand : IRequest<string>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }

        public static DeleteTodoListCommand FromDto(DeleteTodoListDtos dto)
        {
            return new DeleteTodoListCommand
            {
                Id = dto.Id,
                Name = dto.Name,
                Category = dto.Category
            };
        }
    }
}
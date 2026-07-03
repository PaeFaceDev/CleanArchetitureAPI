using MediatR;
using Application.Dtos.TodoList;

namespace Application.Features.TodoList.Queries
{
    public record GetTodoListQuery : IRequest<List<TodoListDtos>>
    {
    }
}
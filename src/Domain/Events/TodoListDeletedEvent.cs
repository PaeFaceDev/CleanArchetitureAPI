using Domain.Common;

namespace Domain.Events
{
    public class TodoListDeletedEvent : DomainEventBase
    {
        public int TodoListId { get; }
        public string Name { get; }
        public string? Category { get; }

        public TodoListDeletedEvent(int todoListId, string name, string? category)
        {
            TodoListId = todoListId;
            Name = name;
            Category = category;
        }
    }
}
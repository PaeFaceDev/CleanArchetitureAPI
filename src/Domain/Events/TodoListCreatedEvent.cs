using Domain.Common;

namespace Domain.Events
{
    public class TodoListCreatedEvent : DomainEventBase
    {
        public int TodoListId { get; }
        public string Name { get; }
        public string? Category { get; }

        public TodoListCreatedEvent(int todoListId, string name, string? category)
        {
            TodoListId = todoListId;
            Name = name;
            Category = category;
        }
    }
}
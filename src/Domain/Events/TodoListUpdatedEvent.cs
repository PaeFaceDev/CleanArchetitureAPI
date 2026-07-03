using Domain.Common;

namespace Domain.Events
{
    public class TodoListUpdatedEvent : DomainEventBase
    {
        public int TodoListId { get; }
        public string OldName { get; }
        public string NewName { get; }
        public string? Category { get; }

        public TodoListUpdatedEvent(int todoListId, string oldName, string newName, string? category)
        {
            TodoListId = todoListId;
            OldName = oldName;
            NewName = newName;
            Category = category;
        }
    }
}
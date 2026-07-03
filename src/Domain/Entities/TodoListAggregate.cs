using Domain.Common;
using Domain.Events;

namespace Domain.Entities
{
    public class TodoListAggregate : EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Category { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private TodoListAggregate() { }

        public TodoListAggregate(string name, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }

            Name = name.Trim();
            Category = category?.Trim();
            CreatedAt = DateTime.UtcNow;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Name cannot be empty", nameof(newName));
            }

            var oldName = Name;
            Name = newName.Trim();
            UpdatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new TodoListUpdatedEvent(Id, oldName, Name, Category));
        }

        public void UpdateCategory(string? newCategory)
        {
            Category = newCategory?.Trim();
            UpdatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new TodoListUpdatedEvent(Id, Name, Name, Category));
        }

        public void MarkAsDeleted()
        {
            AddDomainEvent(new TodoListDeletedEvent(Id, Name, Category));
        }
    }
}
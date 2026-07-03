using System.Threading.Tasks;

namespace Domain.Common
{
    public interface ITodoListNotificationService
    {
        Task NotifyTodoListCreatedAsync(int id, string name, string? category);
        Task NotifyTodoListUpdatedAsync(int id, string oldName, string newName, string? category);
        Task NotifyTodoListDeletedAsync(int id, string name, string? category);
    }
}
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEventsAsync(IEnumerable<IDomainEvent> domainEvents);
    }
}
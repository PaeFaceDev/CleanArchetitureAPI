using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent);
    }
}
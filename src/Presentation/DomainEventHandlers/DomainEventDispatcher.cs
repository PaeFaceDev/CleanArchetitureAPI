using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Microsoft.AspNetCore.SignalR;
using Presentation.SignalRHubs;
using System.Text.Json;

namespace Presentation.DomainEventHandlers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IHubContext<TodoListHub> _hubContext;

        public DomainEventDispatcher(IHubContext<TodoListHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchAndClearEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            if (domainEvents == null || !domainEvents.Any())
            {
                return;
            }

            foreach (var domainEvent in domainEvents)
            {
                await DispatchEventAsync(domainEvent);
            }
        }

        private async Task DispatchEventAsync(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case Domain.Events.TodoListCreatedEvent createdEvent:
                    await _hubContext.Clients.All.SendAsync("TodoListChanged", "create", JsonSerializer.Serialize(new
                    {
                        createdEvent.TodoListId,
                        createdEvent.Name,
                        createdEvent.Category
                    }));
                    break;

                case Domain.Events.TodoListUpdatedEvent updatedEvent:
                    await _hubContext.Clients.All.SendAsync("TodoListChanged", "update", JsonSerializer.Serialize(new
                    {
                        updatedEvent.TodoListId,
                        updatedEvent.OldName,
                        updatedEvent.NewName,
                        updatedEvent.Category
                    }));
                    break;

                case Domain.Events.TodoListDeletedEvent deletedEvent:
                    await _hubContext.Clients.All.SendAsync("TodoListChanged", "delete", JsonSerializer.Serialize(new
                    {
                        deletedEvent.TodoListId,
                        deletedEvent.Name,
                        deletedEvent.Category
                    }));
                    break;

                default:
                    // Handle other domain events if needed
                    break;
            }
        }
    }
}
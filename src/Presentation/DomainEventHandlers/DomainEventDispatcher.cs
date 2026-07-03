using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Microsoft.AspNetCore.SignalR;
using Presentation.SignalRHubs;
using Presentation.Logging;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Presentation.DomainEventHandlers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IHubContext<TodoListHub> _hubContext;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IHubContext<TodoListHub> hubContext, ILogger<DomainEventDispatcher> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task DispatchAndClearEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            try
            {
                if (domainEvents == null || !domainEvents.Any())
                {
                    _logger.LogDebug("No domain events to dispatch");
                    return;
                }

                _logger.LogInformation("Dispatching {EventCount} domain events", domainEvents.Count());
                
                foreach (var domainEvent in domainEvents)
                {
                    await DispatchEventAsync(domainEvent);
                }
                
                _logger.LogInformation("Successfully dispatched {EventCount} domain events", domainEvents.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DispatchAndClearEventsAsync");
                throw;
            }
        }

        private async Task DispatchEventAsync(IDomainEvent domainEvent)
        {
            try
            {
                _logger.LogInformation("Dispatching domain event: {EventType}", domainEvent.GetType().Name);
                LoggingService.LogDomainEvent(domainEvent.GetType().Name, domainEvent);
                
                switch (domainEvent)
                {
                    case Domain.Events.TodoListCreatedEvent createdEvent:
                        _logger.LogInformation("Dispatching TodoListCreatedEvent - Id: {Id}, Name: {Name}", createdEvent.TodoListId, createdEvent.Name);
                        await _hubContext.Clients.All.SendAsync("TodoListChanged", "create", JsonSerializer.Serialize(new
                        {
                            createdEvent.TodoListId,
                            createdEvent.Name,
                            createdEvent.Category
                        }));
                        break;

                    case Domain.Events.TodoListUpdatedEvent updatedEvent:
                        _logger.LogInformation("Dispatching TodoListUpdatedEvent - Id: {Id}, OldName: {OldName}, NewName: {NewName}", updatedEvent.TodoListId, updatedEvent.OldName, updatedEvent.NewName);
                        await _hubContext.Clients.All.SendAsync("TodoListChanged", "update", JsonSerializer.Serialize(new
                        {
                            updatedEvent.TodoListId,
                            updatedEvent.OldName,
                            updatedEvent.NewName,
                            updatedEvent.Category
                        }));
                        break;

                    case Domain.Events.TodoListDeletedEvent deletedEvent:
                        _logger.LogInformation("Dispatching TodoListDeletedEvent - Id: {Id}, Name: {Name}", deletedEvent.TodoListId, deletedEvent.Name);
                        await _hubContext.Clients.All.SendAsync("TodoListChanged", "delete", JsonSerializer.Serialize(new
                        {
                            deletedEvent.TodoListId,
                            deletedEvent.Name,
                            deletedEvent.Category
                        }));
                        break;

                    default:
                        _logger.LogWarning("Unhandled domain event type: {EventType}", domainEvent.GetType().Name);
                        break;
                }
                
                _logger.LogInformation("Successfully dispatched domain event: {EventType}", domainEvent.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DispatchEventAsync for event type: {EventType}", domainEvent.GetType().Name);
                throw;
            }
        }
    }
}
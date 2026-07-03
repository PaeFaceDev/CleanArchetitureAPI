using Microsoft.AspNetCore.SignalR;
using Presentation.Logging;
using Microsoft.Extensions.Logging;

namespace Presentation.SignalRHubs
{
    public class TodoListHub : Hub
    {
        private readonly ILogger<TodoListHub> _logger;

        public TodoListHub(ILogger<TodoListHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var connectionId = Context.ConnectionId;
                var userId = Context.User?.FindFirst("sub")?.Value ?? Context.GetHttpContext()?.Request.Headers["X-User-ID"].FirstOrDefault() ?? "Anonymous";
                
                _logger.LogInformation("SignalR Client Connected - ConnectionId: {ConnectionId}, UserId: {UserId}", connectionId, userId);
                LoggingService.LogSignalRConnection(connectionId, userId, true);
                
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SignalR OnConnectedAsync");
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                var userId = Context.User?.FindFirst("sub")?.Value ?? Context.GetHttpContext()?.Request.Headers["X-User-ID"].FirstOrDefault() ?? "Anonymous";
                
                _logger.LogInformation("SignalR Client Disconnected - ConnectionId: {ConnectionId}, UserId: {UserId}", connectionId, userId);
                LoggingService.LogSignalRConnection(connectionId, userId, false);
                
                if (exception != null)
                {
                    _logger.LogError(exception, "SignalR Disconnection Error - ConnectionId: {ConnectionId}", connectionId);
                }
                
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SignalR OnDisconnectedAsync");
                throw;
            }
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                _logger.LogInformation("SignalR SendMessage - ConnectionId: {ConnectionId}, User: {User}, Message: {Message}", connectionId, user, message);
                LoggingService.LogSignalREvent("SendMessage", connectionId, new { User = user, Message = message });
                
                await Clients.All.SendAsync("ReceiveMessage", user, message);
                
                _logger.LogInformation("SignalR SendMessage completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SignalR SendMessage");
                throw;
            }
        }

        public async Task NotifyTodoListChange(string action, string data)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                _logger.LogInformation("SignalR NotifyTodoListChange - ConnectionId: {ConnectionId}, Action: {Action}, Data: {Data}", connectionId, action, data);
                LoggingService.LogSignalREvent("NotifyTodoListChange", connectionId, new { Action = action, Data = data });
                
                await Clients.All.SendAsync("TodoListChanged", action, data);
                
                _logger.LogInformation("SignalR NotifyTodoListChange completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SignalR NotifyTodoListChange");
                throw;
            }
        }
    }
}
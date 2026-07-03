using Microsoft.AspNetCore.SignalR;

namespace Presentation.SignalRHubs
{
    public class TodoListHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task NotifyTodoListChange(string action, string data)
        {
            await Clients.All.SendAsync("TodoListChanged", action, data);
        }
    }
}
using Serilog;

namespace Presentation.Logging
{
    public static class LoggingService
    {
        public static void LogInformation(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public static void LogWarning(string message, params object[] args)
        {
            Log.Warning(message, args);
        }

        public static void LogError(string message, params object[] args)
        {
            Log.Error(message, args);
        }

        public static void LogError(Exception ex, string message, params object[] args)
        {
            Log.Error(ex, message, args);
        }

        public static void LogDebug(string message, params object[] args)
        {
            Log.Debug(message, args);
        }

        public static void LogUserAction(string userId, string action, string details = "")
        {
            Log.Information("User {UserId} performed action: {Action}. Details: {Details}", userId, action, details);
        }

        public static void LogApiCall(string method, string endpoint, string? userId = null)
        {
            Log.Information("API Call - Method: {Method}, Endpoint: {Endpoint}, User: {UserId}", method, endpoint, userId ?? "Anonymous");
        }

        public static void LogApiError(string method, string endpoint, Exception ex, string? userId = null)
        {
            Log.Error(ex, "API Error - Method: {Method}, Endpoint: {Endpoint}, User: {UserId}", method, endpoint, userId ?? "Anonymous");
        }

        public static void LogSignalRConnection(string connectionId, string userId, bool connected)
        {
            var status = connected ? "Connected" : "Disconnected";
            Log.Information("SignalR {Status} - ConnectionId: {ConnectionId}, UserId: {UserId}", status, connectionId, userId);
        }

        public static void LogSignalREvent(string eventName, string connectionId, object? data = null)
        {
            Log.Information("SignalR Event - Event: {EventName}, ConnectionId: {ConnectionId}, Data: {Data}", eventName, connectionId, data);
        }

        public static void LogCommandEvent(string commandName, object? commandData = null)
        {
            Log.Information("Command Executed - Command: {CommandName}, Data: {CommandData}", commandName, commandData);
        }

        public static void LogQueryExecution(string queryName, object? queryData = null)
        {
            Log.Information("Query Executed - Query: {QueryName}, Data: {QueryData}", queryName, queryData);
        }

        public static void LogDomainEvent(string eventName, object? eventData = null)
        {
            Log.Information("Domain Event - Event: {EventName}, Data: {EventData}", eventName, eventData);
        }
    }
}
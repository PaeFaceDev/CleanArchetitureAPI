using System.Net;
using System.Text.Json;
using Presentation.Logging;
using Serilog;

namespace Presentation.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log incoming request
                LogApiRequest(context);
                
                await _next(context);
                
                // Log response
                LogApiResponse(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private void LogApiRequest(HttpContext context)
        {
            var userId = context.User?.FindFirst("sub")?.Value ?? 
                        context.Request.Headers["X-User-ID"].FirstOrDefault() ?? 
                        "Anonymous";
            
            var method = context.Request.Method;
            var path = context.Request.Path;
            var queryString = context.Request.QueryString.ToString();
            
            LoggingService.LogApiCall(method, $"{path}{queryString}", userId);
            Log.Information("Incoming Request - Method: {Method}, Path: {Path}, Query: {Query}, User: {UserId}", 
                method, path, queryString, userId);
        }

        private void LogApiResponse(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            Log.Information("Response - StatusCode: {StatusCode}", statusCode);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var userId = context.User?.FindFirst("sub")?.Value ?? 
                        context.Request.Headers["X-User-ID"].FirstOrDefault() ?? 
                        "Anonymous";
            
            var method = context.Request.Method;
            var path = context.Request.Path;
            
            // Log the exception
            LoggingService.LogApiError(method, path, exception, userId);
            Log.Error(exception, "Unhandled Exception - Method: {Method}, Path: {Path}, User: {UserId}", 
                method, path, userId);
            
            // Set response details
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);
            
            var response = new
            {
                Status = false,
                Code = context.Response.StatusCode.ToString(),
                Message = GetErrorMessage(exception),
                Details = exception.StackTrace,
                Timestamp = DateTime.UtcNow,
                Path = path,
                Method = method
            };
            
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            await context.Response.WriteAsync(jsonResponse);
        }

        private HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.Conflict,
                TimeoutException => HttpStatusCode.RequestTimeout,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private string GetErrorMessage(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => "Required argument was null",
                ArgumentException => "Invalid argument provided",
                UnauthorizedAccessException => "Access denied",
                KeyNotFoundException => "Requested resource not found",
                InvalidOperationException => "Invalid operation attempted",
                TimeoutException => "Operation timed out",
                _ => "An internal server error occurred"
            };
        }
    }
}
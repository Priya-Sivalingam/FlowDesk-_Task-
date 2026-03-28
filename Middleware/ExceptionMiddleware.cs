using System.Net;
using System.Text.Json;

namespace FlowDesk.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

  public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        // Log different levels based on exception type
        if (ex is KeyNotFoundException or ArgumentException or InvalidOperationException)
            _logger.LogWarning("Handled exception: {Message}", ex.Message);
        else
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

        await HandleExceptionAsync(context, ex);
    }
}

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = ex switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
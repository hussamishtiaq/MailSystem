using System.Net;
using System.Text.Json;

namespace MailSystem.API.Middlewear;

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
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteProblemResponseAsync(context, ex);
        }
    }

    private static Task WriteProblemResponseAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            KeyNotFoundException       => (HttpStatusCode.NotFound,     "Resource not found"),
            ArgumentException          => (HttpStatusCode.BadRequest,   "Invalid request"),
            InvalidOperationException  => (HttpStatusCode.Conflict,     "Operation not allowed"),
            _                          => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)status}",
            title,
            status = (int)status,
            detail = ex.Message,
            traceId = context.TraceIdentifier,
            path = context.Request.Path.ToString()
        };

        context.Response.Clear();
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";

        var payload = JsonSerializer.Serialize(problem,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return context.Response.WriteAsync(payload);
    }
}

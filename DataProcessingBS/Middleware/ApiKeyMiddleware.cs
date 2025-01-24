using DataProcessingBS.Services;

namespace DataProcessingBS.Middleware;

public class ApiKeyMiddleware
{
    private readonly string[] _excludedPaths = new[]
    {
        "/stored-procedure-create-account",
        "/swagger",
        "/swagger/",
        "/swagger/index.html",
        "/swagger/v1/swagger.json"
    };

    private readonly ILogger<ApiKeyMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var requestPath = context.Request.Path.Value?.ToLower();

        if (requestPath != null && _excludedPaths.Any(path =>
                requestPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogDebug($"Skipping API key validation for excluded path: {requestPath}");
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Api-Key", out var apiKey))
        {
            _logger.LogWarning($"API key missing for path: {requestPath}");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Api-Key header is missing");
            return;
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var apiKeyService = scope.ServiceProvider.GetRequiredService<ApiKeyService>();

            try
            {
                if (!await apiKeyService.IsApiKeyValidAsync(apiKey))
                {
                    _logger.LogWarning($"Invalid API key attempt for path: {requestPath}");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid or inactive API key");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating API key");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred while validating the API key");
                return;
            }
        }

        await _next(context);
    }
}
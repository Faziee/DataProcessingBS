namespace DataProcessingBS.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using DataProcessingBS.Services;
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private readonly string[] _excludedPaths = new[] 
        { 
            "/stored-procedure-create-account",    // Account creation endpoint
            "/swagger",               // Swagger UI
            "/swagger/",              // Swagger UI with trailing slash
            "/swagger/index.html",    // Swagger index page
            "/swagger/v1/swagger.json" // Swagger JSON endpoint
        };

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var requestPath = context.Request.Path.Value?.ToLower();
            
            // Check if the current path starts with any of the excluded paths
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
}
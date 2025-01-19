namespace DataProcessingBS.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using DataProcessingBS.Services; // Ensure this matches your actual ApiKeyService location
    using System;
    using System.Threading.Tasks;

    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (!context.Request.Headers.TryGetValue("Api-Key", out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
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
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid or inactive API key");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"An error occurred while validating the API key: {ex.Message}");
                    return;
                }
            }

            await _next(context); // Proceed to next middleware if API key is valid
        }
    }
}

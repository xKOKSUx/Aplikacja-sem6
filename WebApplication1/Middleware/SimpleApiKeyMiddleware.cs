using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Middleware
{
    public class SimpleApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-Key";
        
        private const string EXPECTED_KEY = "secret";

        public SimpleApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            
            if (path.StartsWith("/api", System.StringComparison.OrdinalIgnoreCase))
            {
                if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var providedKey) || providedKey != EXPECTED_KEY)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}

using APIServer.Repository;

namespace APIServer;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;
    private readonly IMemoryDb _memoryDb;

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger, IMemoryDb memoryDb)
    {
        _next = next;
        _logger = logger;
        _memoryDb = memoryDb;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/login"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("GameToken", out var extractedToken))
        {
            _logger.LogWarning("GameToken header not found");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("User-Token header not found");
            return;
        }
        if (!context.Request.Headers.TryGetValue("UserId", out var userId))
        {
            _logger.LogWarning("userId header not found");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("userId header not found");
            return;
        }

        var isValidToken = await _memoryDb.ValidateTokenAsync(extractedToken, userId);

        if (!isValidToken)
        {
            _logger.LogWarning($"Invalid token: {extractedToken}");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        await _next(context);
    }
}
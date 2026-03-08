namespace StoreApi.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
    
   
    
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}

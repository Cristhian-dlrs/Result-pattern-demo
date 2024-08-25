using Nest;
using Serilog.Context;

namespace TechDemo.Api.Middleware;


public class SerilogTracingContext
{
    private readonly RequestDelegate _next;

    public SerilogTracingContext(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public Task InvokeAsync(HttpContext httpContext)
    {
        LogContext.PushProperty("TracingId", httpContext.TraceIdentifier);

        return _next(httpContext);
    }
}

public static class SerilogContextMiddlewareExtensions
{
    public static IApplicationBuilder UserSerilogTracingContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<SerilogTracingContext>();
        return app;
    }
}
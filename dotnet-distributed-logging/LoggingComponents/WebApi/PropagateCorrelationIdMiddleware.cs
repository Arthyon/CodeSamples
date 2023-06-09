using Microsoft.AspNetCore.Http;

namespace LoggingComponents.WebApi;

public class PropagateCorrelationIdMiddleware
{
    readonly RequestDelegate _next;
    
    public PropagateCorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Correlator.Propagate(context.Request);
        await _next.Invoke(context);
    }
}
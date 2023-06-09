using Microsoft.AspNetCore.Builder;

namespace LoggingComponents.WebApi;

public static class PropagateCorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdPropagation(this WebApplication builder) =>
        builder.UseMiddleware<PropagateCorrelationIdMiddleware>();
}
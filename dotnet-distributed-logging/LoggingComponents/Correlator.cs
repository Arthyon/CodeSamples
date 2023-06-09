using Microsoft.AspNetCore.Http;

namespace LoggingComponents;

public static class Correlator
{
    static readonly AsyncLocal<string> AsyncLocalId = new();

    /// <summary>
    /// Gets the current correlation id
    /// </summary>
    public static string? CorrelationId => AsyncLocalId.Value;

    /// <summary>
    /// Propagates correlation for the current execution context.
    /// If given id is null or empty, a correlation id is generated.
    /// </summary>
    /// <param name="correlationId">The correlation id to propagate</param>
    public static void Propagate(string? correlationId)
    {
        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        AsyncLocalId.Value = correlationId;
    }
    
    /// <summary>
    /// Propagates correlation for the current execution context from correlationid header in given request.
    /// If request has no such header, a new correlation id is generated
    /// </summary>
    /// <param name="request">A request with the 'correlationid' header to propagate</param>
    public static void Propagate(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(Constants.CorrelationIdHeader, out var id))
        {
            request.Query.TryGetValue(Constants.CorrelationIdHeader, out id);
        }

        Propagate(id.FirstOrDefault());
    }

    /// <summary>
    /// Starts correlation for the current execution context with a new, unique correlation id.
    /// </summary>
    public static void Start() =>
        Propagate(Guid.NewGuid().ToString());
}
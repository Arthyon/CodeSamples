namespace LoggingComponents.HttpClient;

public class AddCorrelationIdMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(Constants.CorrelationIdHeader, Correlator.CorrelationId);
        return base.SendAsync(request, cancellationToken);
    }
}
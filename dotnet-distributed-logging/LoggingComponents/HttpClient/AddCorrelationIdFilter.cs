using Microsoft.Extensions.Http;

namespace LoggingComponents.HttpClient;

public class AddCorrelationIdFilter : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next) => builder =>
    {
        next(builder);

        builder.AdditionalHandlers.Add(new AddCorrelationIdMessageHandler());
    };
}
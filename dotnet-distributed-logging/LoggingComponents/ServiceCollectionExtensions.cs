using LoggingComponents.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace LoggingComponents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorrelationIdHttpFilter(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddEnumerable(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, AddCorrelationIdFilter>());

        return serviceCollection;
    }
}
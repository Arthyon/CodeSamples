using Microsoft.Extensions.DependencyInjection;

namespace Cache;

public static class ServiceCollectionExtensions
{
    public static void AddCachedService<TInterface, TService, TCacheImplementation>(this IServiceCollection serviceCollection) 
        where TInterface : class
        where TService : class, TInterface 
        where TCacheImplementation : TInterface
    {
        
        serviceCollection.AddSingleton<TService>();
        serviceCollection.AddSingleton<TInterface>(x =>
        {
            TInterface implementation = x.GetRequiredService<TService>();
            return ActivatorUtilities.CreateInstance<TCacheImplementation>(x, implementation);
        });

    }

    public static void AddCache(this IServiceCollection serviceCollection)
    {
        // Or another implementation, e.g. Redis
        serviceCollection.AddDistributedMemoryCache();
    }
}

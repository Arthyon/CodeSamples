using Microsoft.Extensions.Caching.Distributed;

namespace Cache;

public abstract class CacheBase
{
    readonly IDistributedCache _cache;

    protected CacheBase(IDistributedCache cache)
    {
        _cache = cache;
    }

    protected Task<T?> Get<T>(string key, Func<Task<T?>> fetcher, DistributedCacheEntryOptions? options = null)
        where T : class
        => _cache.GetOrUpdate(key, fetcher, options);
}
using Microsoft.Extensions.Caching.Distributed;

namespace Cache.Tests.TestData;

public class CachedTestService : CacheBase, ITestService
{
    public static TimeSpan AbsoluteExpiration = TimeSpan.FromSeconds(1);
    
    readonly ITestService _testService;
    readonly DistributedCacheEntryOptions _options;

    public CachedTestService(ITestService testService, IDistributedCache cache) : base(cache)
    {
        _testService = testService;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpiration
        };
    }

    public Task<string?> GetString(string key) => Get(key, () => _testService.GetString(key), _options);
}
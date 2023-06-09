using Cache.Tests.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace Cache.Tests;

public class CacheTests
{
    readonly IServiceProvider _provider;
    
    public CacheTests()
    {
        var sc = new ServiceCollection();
        
        sc.AddCache();
        
        sc.AddCachedService<ITestService, TestService, CachedTestService>();
        
        _provider = sc.BuildServiceProvider();
    }
    
    [Fact]
    public void CacheIsResolvedFromServiceProvider()
    {
        var service = _provider.GetRequiredService<ITestService>();
        Assert.Equal(typeof(CachedTestService), service.GetType());
    }

    [Fact]
    public async Task ValueIsCached()
    {
        var service = _provider.GetRequiredService<ITestService>();

        const string key = "1";
        var value1 = await service.GetString(key);
        var value2 = await service.GetString(key);
        
        Assert.Equal(value1, value2);
    }
    
    [Fact]
    public async Task ValueInCacheExpires()
    {
        var service = _provider.GetRequiredService<ITestService>();

        const string key = "1";
        var value1 = await service.GetString(key);
        await Task.Delay(CachedTestService.AbsoluteExpiration);
        var value2 = await service.GetString(key);
        
        Assert.NotEqual(value1, value2);
    }
}
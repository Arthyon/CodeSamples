namespace Cache.Tests.TestData;

public class TestService : ITestService
{
    public Task<string?> GetString(string key) => Task.FromResult($"{key}:{Guid.NewGuid()}");
}
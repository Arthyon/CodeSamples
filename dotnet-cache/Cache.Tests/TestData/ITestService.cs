namespace Cache.Tests.TestData;

public interface ITestService
{
    Task<string?> GetString(string key);
}
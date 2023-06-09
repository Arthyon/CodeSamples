using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    readonly IHttpClientFactory _clientFactory;
    readonly string _serviceName;
    readonly string? _dependencyUrl;

    public TestController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _serviceName = Environment.GetEnvironmentVariable("ServiceName") ?? "Unknown service";
        _dependencyUrl = Environment.GetEnvironmentVariable("DependencyUrl");
    }
    
    [HttpGet]
    public async Task Get()
    {
        if (string.IsNullOrEmpty(_dependencyUrl))
        {
            Log.Information("Reached terminating service {ServiceName}", _serviceName);
        }
        else
        {
            Log.Information("Reached service {ServiceName}", _serviceName);
            Log.Information("Propagating from service {ServiceName} to {Dependency}", _serviceName, _dependencyUrl);
            
            using var client = _clientFactory.CreateClient();
            await client.GetAsync($"{_dependencyUrl}/test");
            
            Log.Information("Propagation finished");
        }
    }
}

# Dotnet Distributed Cache Sample

This sample demonstrates an approach to disconnect caching of data from the implementation of the service.

Each service will need a separate cache-implementation in addition to the normal implementation. 
This cache-implementation is a thin pass-through using the available extension methods. See `CachedTestService` for an example.

Given the interface `ITestService`, the concrete implementation `TestService` and the thin cache wrapper `CachedTestService`, the extension method `AddCachedService` will:
- Register `TestService` directly in the DI-container
- Register `CachedTestService` for the interface `ITestService`
- Take control of the construction of `CachedTestService` to inject `TestService` implementation

We utilize `IDistributedCache` to be able to use an implementation agnostic cache that is easy to replace. 

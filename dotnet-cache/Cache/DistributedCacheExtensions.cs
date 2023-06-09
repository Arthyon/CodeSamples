using Microsoft.Extensions.Caching.Distributed;

namespace Cache;

using System;
using System.Text.Json;
using System.Threading.Tasks;

public static class DistributedCacheExtensions
{
    /// <summary>
    /// Gets an object from the specified cache with the specified key.
    /// If the key does not exist, the supplied function is invoked to fetch a fresh copy to cache
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="fetcher">Function invoked to fetch a fresh object when key is empty in the cache</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <typeparam name="T">The type to deserialize cache content to</typeparam>
    /// <returns>The object value from the stored cache key or from the function invocation if key is empty</returns>
    public static async Task<T?> GetOrUpdate<T>(this IDistributedCache cache, string key, Func<Task<T?>> fetcher, DistributedCacheEntryOptions? options = null)
        where T : class
    {
        var data = await Get<T>(cache, key);
        if (data != null)
        {
            return data;
        }

        var newData = await fetcher();
        if (newData != null)
        {
            await Set(cache, key, newData, options);
        }
        return newData;
        
    }

    /// <summary>
    /// Gets an object from the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which the data is stored.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <typeparam name="T">The type to deserialize cache content to</typeparam>
    /// <returns>The object value from the stored cache key or null if key is empty</returns>
    public static async Task<T?> Get<T>(this IDistributedCache cache, string key) where T : class
    {
        var data = await cache.GetStringAsync(key);
        return string.IsNullOrWhiteSpace(data) 
            ? null 
            : JsonSerializer.Deserialize<T>(data); 
    }

    /// <summary>
    /// Sets an object in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="obj">The data to store</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
    public static Task Set<T>(this IDistributedCache cache, string key, T obj, DistributedCacheEntryOptions? options = null) where T : class
    {
        string data = JsonSerializer.Serialize(obj);
        return cache.SetStringAsync(key, data, options ?? new DistributedCacheEntryOptions());
    }
}
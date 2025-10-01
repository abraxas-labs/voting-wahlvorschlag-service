// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Eawv.Service.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Eawv.Service.Services;

public class CacheService : IDisposable
{
    private readonly CacheConfiguration _config;
    private readonly ILogger<CacheService> _logger;
    private readonly IMemoryCache _cache;
    private bool _disposed;

    public CacheService(CacheConfiguration config, ILogger<CacheService> logger)
    {
        _config = config;
        _logger = logger;
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public async Task<T> GetOrCreate<T>(string id, Func<Task<T>> provider)
    {
        return await GetOrCreate(typeof(T).Name, id, provider);
    }

    public async Task<T> GetOrCreate<T, TEntity>(string id, Func<Task<T>> provider)
    {
        return await GetOrCreate(typeof(TEntity).Name, id, provider);
    }

    public async Task<T> GetOrCreate<T>(string keyTypeName, string id, Func<Task<T>> provider)
    {
        var key = CreateCacheKey(keyTypeName, id);
        _logger.LogDebug("Cachekey: {CacheKey}", key);

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            if (!_config.TryGetValue(key, out TimeSpan exp)
                && !_config.TryGetValue(id, out exp)
                && !_config.TryGetValue(keyTypeName, out exp))
            {
                throw new InvalidOperationException($"no cache expiry for {key} specified in appsettings.json");
            }

            entry.SetOptions(new MemoryCacheEntryOptions().SetAbsoluteExpiration(exp));
            return await provider();
        });
    }

    public void Invalidate<T>(string id) => Invalidate(typeof(T).Name, id);

    public void Invalidate(string keyTypeName, string id)
    {
        var key = CreateCacheKey(keyTypeName, id);
        _cache.Remove(key);
    }

    /// <summary>
    /// Disposes unmanaged ressources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes unmanaged ressoures. In this particular case the <see cref="IMemoryCache"/> <see cref="_cache"/>.
    /// </summary>
    /// <param name="disposing">Whether the call comes from a method (true), or from a finalizer (false).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed state (managed objects).
            _cache?.Dispose();
        }

        _disposed = true;
    }

    private string CreateCacheKey(string keyTypeName, string id) => keyTypeName + "." + id;
}

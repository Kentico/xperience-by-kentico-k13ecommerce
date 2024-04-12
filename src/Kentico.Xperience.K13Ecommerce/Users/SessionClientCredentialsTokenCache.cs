using System.Text.Json;

using Duende.AccessTokenManagement;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce.Users;

internal class SessionClientCredentialsTokenCache : IClientCredentialsTokenCache
{
    private readonly IClientCredentialsTokenCache defaultCache;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<SessionClientCredentialsTokenCache> logger;
    private readonly ClientCredentialsTokenManagementOptions options;


    public SessionClientCredentialsTokenCache(
        IClientCredentialsTokenCache defaultCache,
        IOptions<ClientCredentialsTokenManagementOptions> options,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SessionClientCredentialsTokenCache> logger)
    {
        this.defaultCache = defaultCache;
        this.httpContextAccessor = httpContextAccessor;
        this.logger = logger;
        this.options = options.Value;
    }


    public async Task SetAsync(string clientName,
        ClientCredentialsToken clientCredentialsToken,
        TokenRequestParameters requestParameters,
        CancellationToken cancellationToken = default)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            await defaultCache.SetAsync(clientName, clientCredentialsToken, requestParameters, cancellationToken);
            return;
        }
        string data = JsonSerializer.Serialize(clientCredentialsToken);
        string cacheKey = GenerateCacheKey(options, clientName, requestParameters);
        httpContextAccessor.HttpContext.Session.SetString(cacheKey, data);
    }


    public async Task<ClientCredentialsToken?> GetAsync(
        string clientName,
        TokenRequestParameters requestParameters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(clientName);
        if (httpContextAccessor.HttpContext is null)
        {
            return await defaultCache.GetAsync(clientName, requestParameters, cancellationToken);
        }
        string cacheKey = GenerateCacheKey(options, clientName, requestParameters);
        string? entry = httpContextAccessor.HttpContext.Session.GetString(cacheKey);

        if (entry is not null)
        {
            try
            {
                logger.LogDebug("Cache hit for access token for client: {clientName}", clientName);
                return JsonSerializer.Deserialize<ClientCredentialsToken>(entry);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error parsing cached access token for client {clientName}", clientName);
                return null;
            }
        }

        logger.LogTrace("Cache miss for access token for client: {clientName}", clientName);
        return null;
    }


    public async Task DeleteAsync(
        string clientName,
        TokenRequestParameters requestParameters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(clientName);

        string cacheKey = GenerateCacheKey(options, clientName, requestParameters);

        if (httpContextAccessor.HttpContext is not null)
        {
            httpContextAccessor.HttpContext.Session.Remove(cacheKey);
        }
        else
        {
            await defaultCache.DeleteAsync(clientName, requestParameters, cancellationToken);
        }
    }


    /// <summary>
    /// Generates the cache key based on various inputs
    /// </summary>
    /// <param name="options"></param>
    /// <param name="clientName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    protected virtual string GenerateCacheKey(
        ClientCredentialsTokenManagementOptions options,
        string clientName,
        TokenRequestParameters? parameters = null)
    {
        string s = $"s_{parameters?.Scope}" ?? "";
        string r = $"r_{parameters?.Resource}" ?? "";

        return options.CacheKeyPrefix + clientName + "::" + s + "::" + r;
    }
}

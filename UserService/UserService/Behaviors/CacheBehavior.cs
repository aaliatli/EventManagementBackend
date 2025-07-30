using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Caching.Memory;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheable
{
    private readonly IMemoryCache _cache;

    public CacheBehavior(IMemoryCache cache){
        _cache = cache;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = request.CacheKey;
        if (_cache.TryGetValue(cacheKey, out TResponse cachedResponse))
        {
            return cachedResponse;
        }

        var response = await next();

        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(request.CacheDuration));

        _cache.Set(cacheKey, response, cacheOptions);
        return response;
    }
}

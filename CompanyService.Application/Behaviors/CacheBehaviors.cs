using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Behaviors;

/// <summary>Маркер для запитів, результат яких кешується в Redis (cache-aside).</summary>
public interface ICachedQuery {
    string CacheKey { get; }
}

/// <summary>Маркер для команд, після успіху яких треба видалити ключі з Redis.</summary>
public interface ICacheInvalidatingCommand {
    IReadOnlyCollection<string> CacheKeysToInvalidate { get; }
}

/// <summary>
/// Cache-aside у спільному Redis. Жодного локального кешу в пам'яті процесу:
/// будь-який instance бачить той самий кеш. Якщо Redis недоступний — працюємо напряму з БД.
/// </summary>
public class CachingBehavior<TRequest, TResponse>(
    IDistributedCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICachedQuery {

    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) {

        try {
            var cached = await cache.GetAsync(request.CacheKey, cancellationToken);
            if (cached is not null) {
                var value = JsonSerializer.Deserialize<TResponse>(cached, Json);
                if (value is not null) {
                    logger.LogDebug("Cache HIT {Key}", request.CacheKey);
                    return value;
                }
            }
        } catch (Exception ex) {
            logger.LogWarning(ex, "Redis read failed for {Key}, falling back to DB", request.CacheKey);
        }

        var response = await next();

        try {
            if (response is not null) {
                await cache.SetAsync(
                    request.CacheKey,
                    JsonSerializer.SerializeToUtf8Bytes(response, Json),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = Ttl },
                    cancellationToken);
            }
        } catch (Exception ex) {
            logger.LogWarning(ex, "Redis write failed for {Key}", request.CacheKey);
        }

        return response;
    }
}

/// <summary>Інвалідація кешу після успішного виконання команди.</summary>
public class CacheInvalidationBehavior<TRequest, TResponse>(
    IDistributedCache cache,
    ILogger<CacheInvalidationBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheInvalidatingCommand {

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) {

        var response = await next(); // спочатку зміна в БД, потім інвалідація

        foreach (var key in request.CacheKeysToInvalidate) {
            try {
                await cache.RemoveAsync(key, cancellationToken);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Redis invalidation failed for {Key}", key);
            }
        }

        return response;
    }
}

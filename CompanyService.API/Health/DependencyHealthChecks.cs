using CompanyService.Infrastructure.DbContext;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CompanyService.API.Health;

/// <summary>PostgreSQL недоступний => інстанс НЕ готовий обслуговувати запити (503).</summary>
public class PostgresHealthCheck(CompanyDbContext db) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        try
        {
            return await db.Database.CanConnectAsync(cts.Token)
                ? HealthCheckResult.Healthy("PostgreSQL reachable")
                : HealthCheckResult.Unhealthy("PostgreSQL not reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PostgreSQL check failed", ex);
        }
    }
}

/// <summary>Redis недоступний => Degraded (200): сервіс працює напряму з БД, див. CachingBehavior.</summary>
public class RedisHealthCheck(IDistributedCache cache) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        try
        {
            await cache.GetAsync("health:probe", cts.Token);
            return HealthCheckResult.Healthy("Redis reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Redis unavailable, falling back to DB", ex);
        }
    }
}
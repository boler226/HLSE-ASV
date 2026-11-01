using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.DbContext;
using CompanyService.Infrastructure.Repositories;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Infrastructure.Services
{
    public static class CollectionExtensionsService
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            string redisConnectionString)
        {

            // (прибрано Console.WriteLine з рядком підключення: у ньому пароль)

            services.AddDbContext<CompanyDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Redis — спільний розподілений кеш. НЕ використовуємо IMemoryCache / AddDistributedMemoryCache,
            // бо це локальний стан конкретного процесу.
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = redisConnectionString;
                options.InstanceName = "CompanyService:";
            });

            // Repositories & Services (Scoped: живуть рівно один HTTP-запит)
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // MassTransit
            services.AddMassTransit(x => {
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host("rabbitmq", "/", h => {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
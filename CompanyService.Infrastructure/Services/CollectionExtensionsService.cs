using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.DbContext;
using CompanyService.Infrastructure.Repositories;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Infrastructure.Services {
    public static class CollectionExtensionsService {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) {
            Console.WriteLine($"DB CONNECTION: {connectionString}");
            services.AddDbContext<CompanyDbContext>(options => 
                options.UseNpgsql(connectionString));

            // Repositories & Services
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

using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services;
using UsersService.Domain.Entities;
using UsersService.Domain.Interfaces;
using UsersService.Infrastructure.DbContext;
using UsersService.Infrastructure.Repositories;
using UsersService.Infrastructure.UnitOfWork.Interfaces;

namespace UsersService.Infrastructure.Services
{
    public static class CollectionExtensionsService {
         public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string rabbitHost = "rabbitmq") {
            Console.WriteLine($"DB CONNECTION: {connectionString}");
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Identity
            services.AddIdentity<User, IdentityRole<Guid>>(options => {
                options.Stores.MaxLengthForKeys = 128;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

            // MassTransit
            services.AddMassTransit(x => {
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(rabbitHost, "/", h => {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            // Repositories & Services
            services.AddScoped<IEntityExistenceChecker, EntityExistenceChecker>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
         }
    }
}
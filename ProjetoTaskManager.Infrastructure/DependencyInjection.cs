using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Infrastructure.Persistence;
using ProjetoTaskManager.Infrastructure.Repositories;

namespace ProjetoTaskManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureDatabase(configuration);
            services.ConfigureRepositories();
            services.ConfigureServices();
            return services;
        }

        private static IServiceCollection ConfigureDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration
                .GetConnectionString("AppDbConnectionString");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString)));

            return services;
        }

        private static IServiceCollection ConfigureRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            return services;
        }
         private static IServiceCollection ConfigureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IEncryptService, EncryptService>();
            return services;
        }
    }
}
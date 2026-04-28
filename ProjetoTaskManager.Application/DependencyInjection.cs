using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProjetoTaskManager.Application.Mappings;
using ProjetoTaskManager.Application.Services;

namespace ProjetoTaskManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.ConfigureServices();
            services.ConfigureMappings();

            return services;
        }

        private static IServiceCollection ConfigureServices(
            this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<TokenService>();

            return services;
        }

        private static IServiceCollection ConfigureMappings(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMapping));

            return services;
        }
    }
}
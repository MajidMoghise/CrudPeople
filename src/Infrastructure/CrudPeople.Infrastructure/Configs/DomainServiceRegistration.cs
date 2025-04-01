using Microsoft.Extensions.DependencyInjection;

namespace CrudPeople.Infrastructure.Configs
{
    internal static class DomainServiceRegistration
    {
        internal static IServiceCollection AddDomainService(this IServiceCollection services)
        {
            return services.AddSingeltonDomainService()
                           .AddTranseintDomainService()
                           .AddScoppedDomainService();
        }
        private static IServiceCollection AddTranseintDomainService(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddScoppedDomainService(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddSingeltonDomainService(this IServiceCollection services)
        {
            return services;
        }

    }
}

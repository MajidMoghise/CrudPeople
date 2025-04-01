using CrudPeople.ApplicationService;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPeople.Infrastructure.Configs
{
    internal static class ServiceApplicationRegistration
    {
        internal static IServiceCollection AddServiceApplication(this IServiceCollection services)
        {
            return services.AddSingeltonServiceApplication()
                           .AddTranseintServiceApplication()
                           .AddScoppedServiceApplication();
        }
        private static IServiceCollection AddTranseintServiceApplication(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddScoppedServiceApplication(this IServiceCollection services)
        {
            return services.AddScoped<IPeopleService,PeopleService>();
        }
        private static IServiceCollection AddSingeltonServiceApplication(this IServiceCollection services)
        {
            return services;
        }

    }
}

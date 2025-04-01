using CrudPeople.Infrastructure.EfCore.Repositories.People;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPeople.Infrastructure.Configs
{
    internal static class RepositoryRegistration
    {
        internal static IServiceCollection AddRepository(this IServiceCollection services)
        {
            return services.AddSingeltonRepository()
                           .AddTranseintRepository()
                           .AddScoppedRepository();
        }
        private static IServiceCollection AddTranseintRepository(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddScoppedRepository(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddSingeltonRepository(this IServiceCollection services)
        {
            return services;
        }

    }
}

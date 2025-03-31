using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Configs
{
    public static class RegistertaionDatabase
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterSQLDatabase(configuration).RegisterMongoDbDatabase(configuration);
        }
        private static IServiceCollection RegisterMongoDbDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //EventStoreConfiguration.EventStoreConfig()
            return services;
        }

    }
    public static class ServiceApplicationRegistration
    {
        public static IServiceCollection AddServiceApplication(this IServiceCollection services)
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
            return services.AddScoped<IGroupServiceApplication, GroupServiceApplication>()
                           .AddScoped<IPeopleServiceApplication, PeopleServiceApplication>();
        }
        private static IServiceCollection AddSingeltonServiceApplication(this IServiceCollection services)
        {
            return services;
        }

    }
    public static class RepositoryRegistration
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
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
            return services.AddScoped<IGroupsEntityCommandRepository, GroupsEntityCommandRepository>()
                           .AddScoped<IPersonAddressesEntityCommandRepository, PersonAddressesEntityCommandRepository>()
                           .AddScoped<IPersonPhonesEntityCommandRepository, PersonPhonesEntityCommandRepository>()
                           .AddScoped<IPersonEmailsEntityCommandRepository, PersonEmailsEntityCommandRepository>()
                           .AddScoped<IPersonGroupsEntityCommandRepository, PersonGroupsEntityCommandRepository>()
                           .AddScoped<IPeopleEntityCommandRepository, PeopleEntityCommandRepository>()
                           .AddScoped<IGroupsEntityQueryRepository, GroupsEntityQueryRepository>()
                           .AddScoped<IPersonAddressesEntityQueryRepository, PersonAddressesEntityQueryRepository>()
                           .AddScoped<IPersonPhonesEntityQueryRepository, PersonPhonesEntityQueryRepository>()
                           .AddScoped<IPersonEmailsEntityQueryRepository, PersonEmailsEntityQueryRepository>()
                           .AddScoped<IPersonGroupsEntityQueryRepository, PersonGroupsEntityQueryRepository>()
                           .AddScoped<IPeopleEntityQueryRepository, PeopleEntityQueryRepository>()

                           ;
        }
        private static IServiceCollection AddSingeltonRepository(this IServiceCollection services)
        {
            return services;
        }

    }
    public static class DomainServiceRegistration
    {
        public static IServiceCollection AddDomainService(this IServiceCollection services)
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
            return services.AddScoped<IGroupsDomainService, GroupsDomainService>()
                           .AddScoped<IPersonAddressesDomainService, PersonAddressesDomainService>()
                           .AddScoped<IPersonPhonesDomainService, PersonPhonesDomainService>()
                           .AddScoped<IPersonEmailsDomainService, PersonEmailsDomainService>()
                           .AddScoped<IPersonGroupsDomainService, PersonGroupsDomainService>()
                           .AddScoped<IPeopleDomainService, PeopleDomainService>();
        }
        private static IServiceCollection AddSingeltonDomainService(this IServiceCollection services)
        {
            return services;
        }

    }
}

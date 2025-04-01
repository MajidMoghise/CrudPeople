using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Mongo
{
    public static class RegistrationMongoDb
    {
        internal static IServiceCollection RegisterMongoDbDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //EventStoreConfiguration.EventStoreConfig()
            return services;
        }

    }
}

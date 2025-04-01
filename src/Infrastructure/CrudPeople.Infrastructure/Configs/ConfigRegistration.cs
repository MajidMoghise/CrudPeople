using CrudPeople.Infrastructure.EfCore;
using CrudPeople.Infrastructure.Mongo;
using CrudPeople.Infrastructure.Postgre;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Configs
{
    public static class ConfigRegistration
    {
        private static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
           return services.RegisterSQLDatabase(configuration)
                          .RegisterMongoDbDatabase(configuration)
                          .RegisterPostgre(configuration);
           
        }

    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Postgre
{
    internal static class RegistrationPostgre
    {
        internal static IServiceCollection RegisterPostgre(this IServiceCollection services, IConfiguration configuration)
        {
            return services;//.RegisterSQLDatabase(configuration).RegisterMongoDbDatabase(configuration);
        }
    }
}

using CrudPeople.Infrastructure.EfCore.Context.Command;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore;
namespace CrudPeople.Infrastructure.EfCore
{
    internal static class RegistrationEf_SQL
    {
        internal static IServiceCollection RegisterSQLDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var SqlCnnCommand = configuration.GetSection("Connections:Sql:Command").Value;
            var SqlCnnQuery = configuration.GetSection("Connections:Sql:Query").Value;
            services.AddDbContext<Ef_CommandDbContext>(x => x.UseSqlServer(SqlCnnCommand));
            services.AddDbContext<Ef_QueryDbContext>(x => x.UseSqlServer(SqlCnnQuery));
            services.EventStoreConfig(configuration);
            return services;
        }

    }
}

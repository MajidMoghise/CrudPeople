using Elastic.CommonSchema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
            string userName = configuration.GetSection("EventStore:MongoDB:User").Value;
            string password = configuration.GetSection("EventStore:MongoDB:Password").Value;
            string host = configuration.GetSection("EventStore:MongoDB:Host").Value;
            string port = configuration.GetSection("EventStore:MongoDB:Port").Value;
            string database = configuration.GetSection("EventStore:MongoDB:Database").Value;
            var connectionString = $"mongodb://{userName}:{password}@{host}:{port}";
            services.AddSingleton<MongoClient>(provider => new MongoClient(connectionString));
            services.AddScoped(provider =>
            {
                var client = provider.GetRequiredService<MongoClient>();
                return client.GetDatabase(database);
            });
            services.AddScoped(provider =>
            {
                var client = provider.GetRequiredService<MongoClient>();
                return client.StartSession();
            });
            return services;
        }
    }
}

using EventStore.Context;
using EventStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventStore
{
    public static class EventStoreConfiguration
    {
        public static IServiceCollection EventStoreConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticEnabled = Convert.ToBoolean(configuration.GetSection("EventStore:ElasticSearch:IsEnabled").Value);
            var mongoEnabled = Convert.ToBoolean(configuration.GetSection("EventStore:MongoDB:IsEnabled").Value);
            services.AddScoped(sp => new List<EventStoreCollcetionWithModel>());
            //if (elasticEnabled)
            //{
           return services.AddScoped<IEventStore, ElasticSearchContext>();
            //}
           // else if (mongoEnabled)
           // {
           //     services.AddScoped<IEventStore, MongoContext>();
           // }
           
         
        }
    }
}

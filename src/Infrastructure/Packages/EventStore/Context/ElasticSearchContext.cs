using EventStore.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Context
{
    public class ElasticSearchContext: IEventStore
    {
        public IClientSessionHandle Session { get; private set; }
        private readonly ElasticClient _client;
        public ElasticSearchContext(IConfiguration configuration)
        {
            string userName = configuration.GetSection("EventStore:ElasticSearch:UserName").Value;
            string password = configuration.GetSection("EventStore:ElasticSearch:Password").Value;
            string Url = configuration.GetSection("EventStore:ElasticSearch:Url").Value;
            string IndexFormat = configuration.GetSection("EventStore:ElasticSearch:IndexFormat").Value;

            var settings = new ConnectionSettings(new Uri(Url))
            .DefaultIndex(IndexFormat);

            string database = configuration.GetSection("EventStore:Database").Value;
            _client = new ElasticClient(settings);
          

        }
        public void Insert(string collectionName, EventStoreModel entity)
        {
            var doc=new { CollectionName=collectionName, Entity = entity };
            var collection = _client.IndexDocument(doc);
           
        }
        public async Task InsertBulk(List<EventStoreCollcetionWithModel> entities)
        {
            await Task.Run(() => entities.ForEach(f => Insert(f.CollectionName, f.Model)));

        }
        public async Task Insert(EventStoreCollcetionWithModel entity)
        {
            await Task.Run(() => Insert(entity.CollectionName, entity.Model));
        }
    }
}

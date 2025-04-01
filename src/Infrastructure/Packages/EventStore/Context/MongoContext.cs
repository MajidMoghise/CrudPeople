using EventStore.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EventStore.Context
{
    public class MongoContext: IEventStore
    {
        private readonly IMongoDatabase _database;
        public  IClientSessionHandle Session { get; private set; }
        private readonly MongoClient _client;
        public MongoContext(IConfiguration configuration)
        {
            string userName = configuration.GetSection("EventStore:MongoDB:User").Value;
            string password = configuration.GetSection("EventStore:MongoDB:Password").Value;
            string host = configuration.GetSection("EventStore:MongoDB:Host").Value;
            string port = configuration.GetSection("EventStore:MongoDB:Port").Value;
            string database = configuration.GetSection("EventStore:MongoDB:Database").Value;
            _client = new MongoClient("mongodb://" + host + ":" + port);//("mongodb://localhost:27017");
            _database = _client.GetDatabase(database);
            
        }
        public void Insert(string collectionName, EventStoreModel entity)
        {
            var collection = _database.GetCollection<EventStoreModel>(collectionName);
            collection.InsertOne(entity);
        }
        public async Task InsertBulk(List<EventStoreCollcetionWithModel> entities)
        {
            await Task.Run(() =>entities.ForEach(f=>Insert(f.CollectionName,f.Model)));
           
        }
        public async Task Insert(EventStoreCollcetionWithModel entity)
        {
            await Task.Run(()=>Insert(entity.CollectionName, entity.Model));
        }
        
    }
}

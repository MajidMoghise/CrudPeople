using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace EventStore.Models
{
    public class EventStoreModel
    {
        [BsonId]
        public string Id { get; private set; }
        public string OperationType { get; private set; }
        public LogModel BasicData { get; private set; }

        
        public string Data { get; private set; }
        [BsonConstructor]
        public EventStoreModel(string id,string operationType, LogModel basicData, string data)
        {
            OperationType = operationType;
            BasicData = basicData;
            Data = data;
            Id = id;
        }
    }

}
using EventStore.Models;
using EventStore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;

namespace CrudPeople.Infrastructure.Mongo.Context
{
    public class MongoContext
    {
        private readonly IEventStore _eventStore;

        private readonly IMongoDatabase _database;
        public readonly MongoClient _client;
        private readonly List<EventStoreCollcetionWithModel> _mongoModels = new();
        private readonly List<Action> _mongoActions = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionStatus TransactionStatus { get; set; }
        public IClientSessionHandle Transaction { get; set; }

        public MongoContext(
            MongoClient client,
            string databaseName,
            IClientSessionHandle session,
            IHttpContextAccessor httpContextAccessor
,
            IEventStore eventStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _database = client.GetDatabase(databaseName ?? throw new ArgumentNullException(nameof(databaseName)));
            _eventStore = eventStore;
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            return _database.GetCollection<TEntity>(collectionName);
        }

        public void TrackChange<TEntity>(TEntity entity, OperationType operationType, string collectionName)
        {
            _mongoActions.Add(() =>
            {
                var userGuid = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(f => f.Type == "UserId")?.Value;
                Guid userId = Guid.Empty;
                if (userGuid is not null)
                {
                    userId = Guid.Parse(userGuid);
                }

                var logModel = new LogModel
                {
                    DateTime = DateTime.UtcNow,
                    UserId = userId
                };

                var eventStoreModel = new EventStoreModel(
                    Guid.NewGuid().ToString(),
                    operationType.ToString(),
                    logModel,
                    Newtonsoft.Json.JsonConvert.SerializeObject(entity)
                );

                _mongoModels.Add(new EventStoreCollcetionWithModel(collectionName, eventStoreModel));

                var collection = GetCollection<TEntity>(collectionName);

                if (operationType == OperationType.Insert)
                {
                    collection.InsertOne(entity);
                }
                else if (operationType == OperationType.Update)
                {
                    var filter = Builders<TEntity>.Filter.Eq("Id", entity.GetType().GetProperty("Id")?.GetValue(entity));
                    collection.ReplaceOne(filter, entity);
                }
                else if (operationType == OperationType.Delete)
                {
                    var filter = Builders<TEntity>.Filter.Eq("Id", entity.GetType().GetProperty("Id")?.GetValue(entity));
                    collection.DeleteOne(filter);
                }
            });
        }
public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int changesCount = 0;

            try
            {
                Transaction.StartTransaction();
                TransactionStatus=TransactionStatus.Active;
                foreach (var action in _mongoActions)
                {
                    action.Invoke();
                    changesCount++;
                }

                if (_mongoModels.Any())
                {
                    await InsertIntoEventStoreAsync(cancellationToken);
                }

                await Transaction.CommitTransactionAsync();
                TransactionStatus = TransactionStatus.Committed;

            }
            catch
            {
                await Transaction.AbortTransactionAsync();
                TransactionStatus = TransactionStatus.Aborted;

                throw;
            }

            return changesCount;
        }

        private async Task InsertIntoEventStoreAsync(CancellationToken cancellationToken)
        {
            foreach (var model in _mongoModels)
            {
                await Task.Run(() =>
                {
                     _eventStore.Insert(model.CollectionName,model.Model);
                });
            }
        }
    }

    
}


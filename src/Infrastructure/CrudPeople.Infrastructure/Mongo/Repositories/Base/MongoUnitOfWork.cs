using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.Infrastructure.EfCore.Context.Command;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using EventStore.Models;
using EventStore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using CrudPeople.Infrastructure.Mongo.Context;

namespace CrudPeople.Infrastructure.Mongo.Repositories.Base
{

    public class MongoUnitOfWork : IUnitOfWork, IDisposable
    {
        public MongoContext _mongoContext;
        private readonly IMongoDatabase _database;
        private readonly List<EventStoreCollcetionWithModel> _mongoModels;
        private readonly IEventStore _eventStore;

        public bool Disposed { get; private set; } = false;

        public MongoUnitOfWork(MongoContext context, List<EventStoreCollcetionWithModel> mongoModels, IEventStore eventStore)
        {
            _mongoModels = mongoModels;
            _eventStore = eventStore;

            _mongoContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task BeginTransactionAsync()
        {
            if (_mongoContext.Transaction == null || !_mongoContext.Transaction.IsInTransaction)
            {
                _mongoContext.Transaction = await Task.FromResult(_mongoContext._client.StartSession());
                _mongoContext.Transaction.StartTransaction();
                _mongoContext.TransactionStatus = TransactionStatus.Active;

            }
        }

        public void BeginTransaction()
        {
            if (_mongoContext.Transaction == null || !_mongoContext.Transaction.IsInTransaction)
            {
                _mongoContext.Transaction = _mongoContext._client.StartSession();
                _mongoContext.Transaction.StartTransaction();
                _mongoContext.TransactionStatus=TransactionStatus.Active;
            }
        }

        public async Task CommitAsync()
        {
            if (_mongoContext.Transaction != null && _mongoContext.Transaction.IsInTransaction)
            {
                await _mongoContext.Transaction.CommitTransactionAsync();
                _mongoContext.TransactionStatus = TransactionStatus.Committed;

            }
        }

        public void Commit()
        {
            if (_mongoContext.Transaction != null && _mongoContext.Transaction.IsInTransaction)
            {
                _mongoContext.Transaction.CommitTransaction();
                _mongoContext.TransactionStatus = TransactionStatus.Committed;
            }
        }

        public async Task RollBackAsync()
        {
            if (_mongoContext.Transaction != null && _mongoContext.Transaction.IsInTransaction)
            {
                await _mongoContext.Transaction.AbortTransactionAsync();
                _mongoContext.TransactionStatus = TransactionStatus.Aborted;

            }
        }

        public void RollBack()
        {
            if (_mongoContext.Transaction != null && _mongoContext.Transaction.IsInTransaction)
            {
                _mongoContext.Transaction.AbortTransaction();
                _mongoContext.TransactionStatus = TransactionStatus.Aborted;
            }
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                _mongoContext.Transaction?.Dispose();
                _mongoContext.TransactionStatus = TransactionStatus.InDoubt;

                Disposed = true;
            }
        }
    }

}

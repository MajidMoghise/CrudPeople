using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.Infrastructure.EfCore.Context.Command;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using EventStore;
using EventStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;

namespace CrudPeople.Infrastructure.EfCore.Repositories.Base.Commands.Base
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        public bool Disposed { get; set; } = false;

        private readonly List<EventStoreCollcetionWithModel> _mongoModels;
        private readonly IEventStore _eventStore;
        public EFUnitOfWork(Ef_CommandDbContext context, Ef_QueryDbContext queryContext, List<EventStoreCollcetionWithModel> mongoModels, IEventStore eventStore)
        {
            CommandContext = context ?? throw new ArgumentNullException(nameof(context));
            QueryContext = queryContext ?? throw new ArgumentNullException(nameof(queryContext));
            _mongoModels = mongoModels;
            _eventStore = eventStore;
        }

        public Ef_CommandDbContext CommandContext { get; }
        public Ef_QueryDbContext QueryContext { get; }

        public async Task BeginTransactionAsync()
        {

            if (CommandContext.Transaction == null)
            {
                CommandContext.Transaction = await CommandContext.Database.BeginTransactionAsync();
                CommandContext.TransactionStatus = TransactionStatus.Active;
            }
            if (QueryContext.Transaction == null)
            {
                QueryContext.Transaction = await QueryContext.Database.BeginTransactionAsync();
                QueryContext.TransactionSatatus = TransactionStatus.Active;
            }


        }
        public void BeginTransaction()
        {
            if (CommandContext.Transaction == null)
            {
                CommandContext.Transaction = CommandContext.Database.BeginTransaction();
                CommandContext.TransactionStatus = TransactionStatus.Active;
            }
            if (QueryContext.Transaction == null)
            {
                QueryContext.Transaction = QueryContext.Database.BeginTransaction();
                QueryContext.TransactionSatatus = TransactionStatus.Active;
            }
        }


        public async Task CommitAsync()
        {
            if (CommandContext.Transaction is not null && CommandContext.TransactionStatus == TransactionStatus.Active)
            {
                await CommandContext.Transaction.CommitAsync();
                CommandContext.TransactionStatus = TransactionStatus.Committed;
                await CommandContext.Transaction.DisposeAsync();
                CommandContext.TransactionStatus = TransactionStatus.InDoubt;
                Disposed = true;
                CommandContext.Transaction = null;
                await _eventStore.InsertBulk(_mongoModels);
            }
            if (QueryContext.Transaction is not null && QueryContext.TransactionSatatus == TransactionStatus.Active)
            {
                await QueryContext.Transaction.CommitAsync();
                QueryContext.TransactionSatatus = TransactionStatus.Committed;
                await QueryContext.Transaction.DisposeAsync();
                QueryContext.TransactionSatatus = TransactionStatus.InDoubt;
                Disposed = true;
                QueryContext.Transaction = null;

            }

        }
        public void Commit()
        {
            if (CommandContext.Transaction is not null && CommandContext.TransactionStatus == TransactionStatus.Active)
            {
                CommandContext.Transaction.Commit();
                CommandContext.TransactionStatus = TransactionStatus.Committed;
                CommandContext.Transaction.Dispose();
                CommandContext.TransactionStatus = TransactionStatus.InDoubt;
                Disposed = true;
                CommandContext.Transaction = null;
                _eventStore.InsertBulk(_mongoModels).GetAwaiter();
            }
            if (QueryContext.Transaction is not null && QueryContext.TransactionSatatus == TransactionStatus.Active)
            {
                QueryContext.Transaction.Commit();
                QueryContext.TransactionSatatus = TransactionStatus.Committed;
                QueryContext.Transaction.Dispose();
                QueryContext.TransactionSatatus = TransactionStatus.InDoubt;
                Disposed = true;
                QueryContext.Transaction = null;
            }
        }

        public void Dispose()
        {
            if (CommandContext.Transaction is not null)
            {
                CommandContext.Transaction.Dispose();
                CommandContext.TransactionStatus = TransactionStatus.InDoubt;
                Disposed = true;
            }
            if (QueryContext.Transaction is not null)
            {
                QueryContext.Transaction.Dispose();
                QueryContext.TransactionSatatus = TransactionStatus.InDoubt;
                Disposed = true;
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {

                }
                else
                {
                    CommandContext.Dispose();
                    QueryContext.Dispose();
                }

            }

            Disposed = true;
        }


        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            CommandContext.ChangeTracker.TrackGraph(rootEntity, callback);
        }

        public void RollBack()
        {
            if (CommandContext.Transaction is not null && CommandContext.TransactionStatus == TransactionStatus.Active)
            {
                CommandContext.Transaction?.Rollback();
                CommandContext.TransactionStatus = TransactionStatus.Aborted;
            }
            if (QueryContext.Transaction is not null && QueryContext.TransactionSatatus == TransactionStatus.Active)
            {
                QueryContext.Transaction.Rollback();
                QueryContext.TransactionSatatus = TransactionStatus.Aborted;
            }
        }

        public async Task RollBackAsync()
        {
            if (CommandContext.Transaction is not null && CommandContext.TransactionStatus == TransactionStatus.Active)
            {
                await CommandContext.Transaction?.RollbackAsync();
                CommandContext.TransactionStatus = TransactionStatus.Aborted;
            }
            if (QueryContext.Transaction is not null && QueryContext.TransactionSatatus == TransactionStatus.Active)
            {
                await QueryContext.Transaction.RollbackAsync();
                QueryContext.TransactionSatatus = TransactionStatus.Aborted;
            }
        }


    }
}

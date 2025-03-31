using CrudPeople.CoreDomain.Entities;
using CrudPeople.CoreDomain.Entities.People.Command;
using CrudPeople.Infrastructure.EfCore.Context.ModelCreating;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using EventStore;
using EventStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Transactions;

namespace CrudPeople.Infrastructure.EfCore.Context.Command
{
    public partial class Ef_CommandDbContext(
            DbContextOptions<Ef_CommandDbContext> options,
            IConfiguration configuration,
            IEventStore eventStore,
            IHttpContextAccessor httpContextAccessor,
            Ef_QueryDbContext queryContext,
            List<EventStoreCollcetionWithModel> mongoModels,
            Microsoft.Extensions.Logging.ILoggerFactory logger
            ) : DbContext(options)
    {
        private readonly string _connectionString = configuration.GetSection("Connections:SqlCommand").Value.ToString();
        private readonly IEventStore _eventStore = eventStore;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IConfiguration _configuration = configuration;
        private readonly Ef_QueryDbContext _queryContext = queryContext;
        private readonly List<EventStoreCollcetionWithModel> _mongoModels = mongoModels;
        private readonly Microsoft.Extensions.Logging.ILoggerFactory _logger = logger;

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString)                       
                          .EnableSensitiveDataLogging().UseLoggerFactory(_logger);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.MainCommandConfig(_configuration);

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<Func<bool>> testkchangeQueryDbs = new();
            List<Action> actionAddMongos = new();

            foreach (var e in ChangeTracker.Entries())
            {
                var typeEntity = e.Entity.GetType();
                OperationType oprType = default;

                var userGuid = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(f => f.Type == "UserId")?.Value;
                Guid userId = Guid.Empty;
                if (userGuid is not null)
                {
                    userId = Guid.Parse(userGuid);
                }

                var lgModel = new LogModel
                {
                    DateTime = DateTime.UtcNow,
                    UserId = userId
                };
                if (e.State == EntityState.Added)
                {
                    oprType = OperationType.Insert;
                    testkchangeQueryDbs.Add(AddToDbQuery(e, e.Metadata.GetTableName()));
                }
                else if (e.State == EntityState.Modified)
                {
                    oprType = OperationType.Update;

                    testkchangeQueryDbs.Add(UpdateToDbQuery(e, e.Metadata.GetTableName()));
                }


                actionAddMongos.Add(() =>
               {
                   _mongoModels.Add(
                       new EventStoreCollcetionWithModel(
                           e.Metadata.GetTableName(),
                           new EventStoreModel(
                               Guid.NewGuid().ToString(),
                               oprType.ToString(),
                               lgModel,
                               Newtonsoft.Json.JsonConvert.SerializeObject(e.Entity)
                               )
                           )
                       );
               });

            }
            int result = 0;
            try
            {
                result = await base.SaveChangesAsync(cancellationToken);
                actionAddMongos.ForEach(f => f.Invoke());
                if (changeQueryDb)
                {
                    testkchangeQueryDbs.ForEach(f =>
                    {
                        var result = f.Invoke();
                        if (result)
                        {
                            _queryContext.SaveChanges();
                        }

                    }
                    );
                }

                // await _eventStore.InsertBulk(mongoModels);

            }
            catch
            {
                await Transaction.RollbackAsync();
                TransactionStatus = TransactionStatus.Aborted;

                await _queryContext.Transaction.RollbackAsync();
                _queryContext.TransactionSatatus = TransactionStatus.Aborted;

                throw;
            }
            return result;
        }

        public override int SaveChanges()
        {
            List<Func<bool>> testkchangeQueryDbs = new();
            List<EventStoreCollcetionWithModel> mongoModels = new();
            List<Action> actionAddMongos = new();
            foreach (var e in ChangeTracker.Entries())
            {
                var typeEntity = e.Entity.GetType();
                OperationType oprType = default;
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(f => f.Type == "UserId").Value);
                var lgModel = new LogModel
                {
                    DateTime = DateTime.UtcNow,
                    UserId = userId
                };
                if (e.State == EntityState.Added)
                {
                    oprType = OperationType.Insert;
                    testkchangeQueryDbs.Add(AddToDbQuery(e, e.Metadata.GetTableName()));
                }
                else if (e.State == EntityState.Modified)
                {
                    oprType = OperationType.Update;
                    testkchangeQueryDbs.Add(UpdateToDbQuery(e, e.Metadata.GetTableName()));
                }

                actionAddMongos.Add(() =>
                {
                    mongoModels.Add(
                    new EventStoreCollcetionWithModel(
                        e.Metadata.GetTableName(),
                        new EventStoreModel(
                            Guid.NewGuid().ToString(),
                            oprType.ToString(),
                            lgModel,
                            Newtonsoft.Json.JsonConvert.SerializeObject(e.Entity)
                            )
                        )
                    );
                });
            }
            int result = 0;
            try
            {
                result = base.SaveChanges();
                actionAddMongos.ForEach(f => f.Invoke());
                if (changeQueryDb)
                {
                    testkchangeQueryDbs.ForEach(f =>
                    {
                        var result = f.Invoke();
                        if (result)
                        {
                            _queryContext.SaveChanges();
                        }

                    }
                    );
                }
                Task.Run(() => _eventStore.InsertBulk(mongoModels));
            }
            catch
            {
                base.Database.RollbackTransaction();
                _queryContext.Database.RollbackTransaction();
                throw;
            }
            return result;
        }
        public void SQLFrom(string sql)
        {
            SQLFrom(sql);
        }


        public DbSet<PeopleCommandEntity> People { get; set; }
        public DbSet<PersonType> PersonType { get; set; }

    }
}

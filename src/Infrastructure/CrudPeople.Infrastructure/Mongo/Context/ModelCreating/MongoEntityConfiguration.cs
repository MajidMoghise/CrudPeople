namespace CrudPeople.Infrastructure.Mongo.Context.ModelCreating
{
    public abstract class MongoEntityConfiguration<TEntity>
    {
        public abstract void Configure(MongoMappingBuilder<TEntity> builder);
    }
}

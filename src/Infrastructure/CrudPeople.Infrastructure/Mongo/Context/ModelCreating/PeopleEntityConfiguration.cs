using CrudPeople.CoreDomain.Entities.People.Query;

namespace CrudPeople.Infrastructure.Mongo.Context.ModelCreating
{
    public class PeopleEntityConfiguration : MongoEntityConfiguration<PeopleQueryEntity>
    {
        public override void Configure(MongoMappingBuilder<PeopleQueryEntity> builder)
        {
            builder.ToCollection("People");

            builder.WithIndex(entity => entity.NationalCode); 
            builder.WithIndex(entity => entity.PersonTypeId); }
    }
}

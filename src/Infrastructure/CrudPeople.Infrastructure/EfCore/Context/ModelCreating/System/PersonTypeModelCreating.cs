using Microsoft.EntityFrameworkCore;
namespace CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Systems
{
    internal class PersonTypeModelCreating
    {
        public const string NameOfTable = "PersonType";
        public PersonTypeModelCreating()
        {

        }
        internal void Config(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<CoreDomain.Entities.PersonType>(entity =>
            {
                entity.ToTable("PersonType", "people.system");
                entity.HasData(new CoreDomain.Entities.PersonType
                {
                    Id = (byte)CoreDomain.Enums.PersonType.Legal,
                    Name = CoreDomain.Enums.PersonType.Legal.ToString(),
                });
                entity.HasData(new CoreDomain.Entities.PersonType
                {
                    Id = (byte)CoreDomain.Enums.PersonType.Individual,
                    Name = CoreDomain.Enums.PersonType.Individual.ToString(),
                });
            });

        }
    }
}

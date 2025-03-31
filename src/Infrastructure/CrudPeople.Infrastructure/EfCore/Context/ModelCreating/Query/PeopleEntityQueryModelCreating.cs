using CrudPeople.CoreDomain.Entities.People.Query;
using Microsoft.EntityFrameworkCore;

namespace CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Queries
{
    internal class PeopleQueryEntityModelCreating
    {
        public const string NameOfTable = "People";
        public PeopleQueryEntityModelCreating()
        {

        }
        internal void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeopleQueryEntity>(entity =>
            {
                entity.ToTable("People", "people.query");


                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();
                


            });


        }
    }
}

using CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Commands;
using CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Queries;
using CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Systems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrudPeople.Infrastructure.EfCore.Context.ModelCreating
{
    internal static class ModelCreating
    {
        internal static void MainCommandConfig(this ModelBuilder modelBuilder, IConfiguration configuration)
        {
            new PeopleCommandEntityModelCreating().Config(modelBuilder);
            new PersonTypeModelCreating().Config(modelBuilder);
            

        }
        internal static void MainQueryConfig(this ModelBuilder modelBuilder, IConfiguration configuration)
        {
            new PeopleQueryEntityModelCreating().Config(modelBuilder);
            
        }
    }
}

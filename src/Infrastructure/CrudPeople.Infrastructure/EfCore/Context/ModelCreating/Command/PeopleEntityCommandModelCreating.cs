using CrudPeople.CoreDomain.Entities.People.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Commands
{
    internal class PeopleCommandEntityModelCreating
    {
        
       
        public const string NameOfTable = "People";
        internal void Config(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<PeopleCommandEntity>(entity =>
            {

                entity.ToTable("People", "people.command");

                entity.Property(e => e.BirthDate)
                      .IsRequired(true);
                entity.Property(e => e.NationalCode)
                      .HasMaxLength(11)
                      .IsRequired(true);
                 entity.Property(e => e.PersonTypeId)
                      .IsRequired(true);
                entity.HasKey(x => x.Id);
                entity.Property(e => e.DoingUserName)
                      .HasMaxLength(100)
                      .IsRequired(true);
                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();


                
                entity.Property(e => e.ChangeDate)
                      .ValueGeneratedOnAdd()
                      .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Local));
             
            
                entity.Property(e => e.RowVersion)
                      .IsRowVersion();

                entity.Property(e => e.FirstName).IsRequired(true)
                  .HasMaxLength(20);
                entity.Property(e => e.LastName).IsRequired(true)
                  .HasMaxLength(20);
               
                entity.HasOne(e => e.PersonType)
                      .WithMany()
                      .HasForeignKey(e => e.PersonTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

            });


        }
    }
}

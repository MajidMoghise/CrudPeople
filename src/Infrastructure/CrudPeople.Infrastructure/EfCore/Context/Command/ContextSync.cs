using CrudPeople.CoreDomain.Entities.People.Command;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.Infrastructure.EfCore.Context.ModelCreating.Commands;
using Helpers.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using MongoDB.Driver;
using Nest;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.EfCore.Context.Command
{
    public partial class Ef_CommandDbContext : DbContext
    {
        private bool changeQueryDb = false;
        private Func<bool> AddToDbQuery(EntityEntry entity, string commandTableName)
        {

            if (string.Equals(commandTableName, PeopleCommandEntityModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PeopleQueryEntity(entity);
            }
            
            return () => { return false; };
        }
        private Func<bool> UpdateToDbQuery(EntityEntry entity, string commandTableName)
        {
            if (string.Equals(commandTableName, PeopleCommandEntityModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return UpdatePeopleQueryEntity(entity);
            }
           
            return () => { return false; };

        }



        private Func<bool> PeopleQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PeopleCommandEntity)entity.Entity;
                var personType = (CrudPeople.CoreDomain.Enums.PersonType)entity.GetDatabaseValues().GetValue<byte>(nameof(PeopleCommandEntity.PersonTypeId));
                _queryContext.AddAsync(new PeopleQueryEntity
                {
                    PersonTypeName = personType.GetEnumName(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    NationalCode = e.NationalCode,
                    PersonTypeId = e.PersonTypeId,
                    BirthDate = e.BirthDate,
                    FullName=e.FirstName+" "+e.LastName,
                    RowVersion=e.RowVersion,
                    Id = e.Id,
                    DoingUserName = e.DoingUserName,
                }).GetAwaiter();

                return true;
            };

        }
        private Func<bool> UpdatePeopleQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PeopleCommandEntity)entity.Entity;
                var edited = false;
                var data = _queryContext.People.AsNoTracking().FirstOrDefaultAsync(f => f.Id == e.Id).Result;
                if (data is null)
                {
                    throw new Exception("In query Database Entity not found");
                }
                var at = _queryContext.People.Attach(data);
                if (data.FirstName != e.FirstName)
                {
                    edited = true;
                    data.FirstName = e.FirstName;
                    at.Property(p => p.FirstName).IsModified = true;
                }
                if(!string.Equals(data.FirstName,e.FirstName)||!string.Equals(data.LastName, e.LastName))
                {
                    edited = true;
                    data.FullName = e.FirstName + " " + e.LastName;
                    at.Property(p => p.FullName).IsModified = true;
                }
                if (data.NationalCode != e.NationalCode)
                {
                    edited = true;
                    data.NationalCode = e.NationalCode;
                    at.Property(p => p.NationalCode).IsModified = true;
                }
                if (data.LastName != e.LastName)
                {
                    edited = true;
                    data.LastName = e.LastName;
                    at.Property(p => p.LastName).IsModified = true;
                }
                if (data.BirthDate!= e.BirthDate)
                {
                    edited = true;
                    data.BirthDate = e.BirthDate;
                    at.Property(p => p.BirthDate).IsModified = true;
                }
                

                if (data.PersonTypeId != e.PersonTypeId)
                {
                    edited = true;
                    data.PersonTypeId = e.PersonTypeId;
                    at.Property(p => p.PersonTypeId).IsModified = true;
                    var personName = e.PersonTypeId.GetEnumName<CrudPeople.CoreDomain.Enums. PersonType, byte>(true);
                    data.PersonTypeName = personName;
                    at.Property(p => p.PersonTypeName).IsModified = true;

                }
                if (edited)
                {
                    data.RowVersion = e.RowVersion;
                    at.Property(p => p.RowVersion).IsModified = true;
                }
                return true;
            };
        }
        
        private Func<bool> DeletePeopleQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PeopleCommandEntity)entity.Entity;

                var data = _queryContext.People.AsNoTracking().Where(w => w.Id == e.Id).ToListAsync().Result;
                if (!data.Any())
                {
                    throw new Exception("In query Database Entity not found");
                }
                foreach (var item in data)
                {
                    _queryContext.People.Remove(item);
                }

                return true;
            };
        }
        
    }

}

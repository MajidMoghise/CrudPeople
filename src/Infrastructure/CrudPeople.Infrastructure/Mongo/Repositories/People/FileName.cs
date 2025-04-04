using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Entities.People.Command;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Mongo.Repositories.People
{
    internal class PeopleMongoRepositoryMapper
    {
        internal PeopleQueryEntity PeopleCommandEntity(CreatePersonRequestModel entity)
        {
            return new PeopleQueryEntity
            {
                
                BirthDate = entity.BirthDate,
                DoingUserName = entity.DoingUserName,
                FirstName = entity.FirstName,
                FullName = entity.FirstName + " " + entity.LastName,
                Id = Guid.NewGuid(),
                LastName = entity.LastName,
                NationalCode = entity.NationalCode,
                PersonTypeId = entity.PersonTypeId,
                PersonTypeName = Helpers.Extentions.EnumExtensions.GetEnumName<PersonType>((PersonType)entity.PersonTypeId),
                RowVersion=Guid.NewGuid().ToByteArray(),
            }; 
                
        }

        internal PeopleResponseModel ToPeopleResponseModel(PeopleQueryEntity entity, int arg2)
        {
            return new PeopleResponseModel
            {
                BirthDate = entity.BirthDate,
                FirstName = entity.FirstName,
                Id = entity.Id,
                LastName = entity.LastName,
                NationalCode = entity.NationalCode,
                PersonTypeId = entity.PersonTypeId,
                PersonTypeName = entity.PersonTypeName,
                RowVersion = entity.RowVersion,
            };
        }

        internal PersonResponseModel ToPersonResponseModel(PeopleQueryEntity person)
        {
            return new PersonResponseModel
            {
                BirthDate = person.BirthDate,
                FirstName = person.FirstName,
                Id = person.Id,
                LastName = person.LastName,
                NationalCode = person.NationalCode,
                PersonTypeId = person.PersonTypeId,
                PersonTypeName = person.PersonTypeName,
                RowVersion = person.RowVersion,
            };
        }
    }
}

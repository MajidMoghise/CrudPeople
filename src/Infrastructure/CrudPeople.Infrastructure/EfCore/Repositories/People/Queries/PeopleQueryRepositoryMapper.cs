
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Entities.People.Query;

namespace CrudPeople.Infrastructure.EfCore.Repositories.People.Queries
{
    internal class PeopleQueryRepositoryMapper
    {
        internal PeopleResponseModel PeopleResponseModel(PeopleQueryEntity s)
        {
            return new PeopleResponseModel
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                NationalCode = s.NationalCode,
                RowVersion = s.RowVersion,
                Id = s.Id,
                BirthDate = s.BirthDate,
                PersonTypeId = s.PersonTypeId,
                PersonTypeName= s.PersonTypeName    
            };
        } internal PersonResponseModel PersonResponseModel(PeopleQueryEntity s)
        {
            return new PersonResponseModel
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                NationalCode = s.NationalCode,
                RowVersion = s.RowVersion,
                Id = s.Id,
                BirthDate = s.BirthDate,
                PersonTypeId = s.PersonTypeId,
                PersonTypeName= s.PersonTypeName    
            };
        }
    }
}

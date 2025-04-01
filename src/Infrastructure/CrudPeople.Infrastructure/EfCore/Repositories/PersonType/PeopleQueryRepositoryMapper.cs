
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Contracts.PersonType.Models;
using CrudPeople.CoreDomain.Entities;
using CrudPeople.CoreDomain.Entities.People.Query;

namespace CrudPeople.Infrastructure.EfCore.Repositories.PersonType
{
    internal class PeopleQueryRepositoryMapper
    {
        internal PersonTypeResponseModel PersonTypeResponseModel(CoreDomain.Entities.PersonType s)
        {
            return new PersonTypeResponseModel
            {
                Id = s.Id,
                Name=s.Name,
            };
        }
    }
}

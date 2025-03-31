using People.Domain.Contract.Repositories.People.Query.Modles;
using People.Domain.DataModels.DataModels.Queries;

namespace IDP.Infrastructure.Repositories.Attributes.Queries
{
    internal class PeopleQueryRepositoryMapper
    {
        internal PeopleGetResponseModel PeopleGetResponseModel(PeopleDataEntityQuery s)
        {
            return new PeopleGetResponseModel
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                NationalCode = s.NationalCode,
                PersonRowVersion = s.PersonRowVersion,
                PersonId = s.Id
            };
        }
    }
}

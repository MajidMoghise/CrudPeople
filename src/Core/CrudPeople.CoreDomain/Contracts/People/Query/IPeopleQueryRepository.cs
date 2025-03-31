using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Contracts.PersonType.Models;
using Helpers.FilterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Contracts.People.Query
{
    public interface IPeopleQueryRepository
    {
        Task<SearchResponseModel<PeopleResponseModel>> GetList(SearchRequestModel<PeopleRequestModel, PeopleResponseModel> request);
        Task<PersonResponseModel> GetById(Guid id);
    }
}

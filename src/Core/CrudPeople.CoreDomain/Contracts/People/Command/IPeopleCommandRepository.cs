using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Contracts.PersonType.Models;
using Helpers.FilterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Contracts.People.Command
{
    public interface IPeopleCommandRepository
    {
        Task<Guid> CreatePerson(CreatePersonRequestModel request);
        Task UpdatePerson(UpdatePersonRequestModel request);
        Task DeletePerson(DeletePersonRequestModel request);
    }
}

using Helpers.FilterSearch;

namespace CrudPeople.ApplicationService
{ 

    internal class PeopleService : IPeopleService
    {
        public Task<Guid> CreatePerson(CreatePersonRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task DeletePerson(DeletePersonRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<PersonResponseDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResponseModel<PeopleResponseDto>> GetList(SearchRequestModel<PeopleRequestDto, PeopleResponseDto> request)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePerson(UpdatePersonRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}

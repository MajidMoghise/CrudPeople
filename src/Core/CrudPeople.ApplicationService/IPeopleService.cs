using Helpers.FilterSearch;

namespace CrudPeople.ApplicationService
{
    public interface IPeopleService
    {
        Task<SearchResponseModel<PeopleResponseDto>> GetList(SearchRequestModel<PeopleRequestDto, PeopleResponseDto> request);
        Task<PersonResponseDto> GetById(Guid id);
        Task<Guid> CreatePerson(CreatePersonRequestDto request);
        Task UpdatePerson(UpdatePersonRequestDto request);
        Task DeletePerson(DeletePersonRequestDto request);
    }
}

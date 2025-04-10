using CrudPeople.CoreDomain.Contracts.People.Command;
using CrudPeople.CoreDomain.Contracts.People.Query;
using Helpers.FilterSearch;

namespace CrudPeople.ApplicationService
{
    internal class PeopleService : IPeopleService
    {
        private readonly PeopleServiceMapper _mapper;
        private readonly IPeopleQueryRepository _peopleQueryRepository;
        private readonly IPeopleCommandRepository _peopleCommandRepository;
        public PeopleService(IPeopleQueryRepository peopleQueryRepository, IPeopleCommandRepository peopleCommandRepository)
        {
            _mapper = new PeopleServiceMapper();
            _peopleQueryRepository = peopleQueryRepository;
            _peopleCommandRepository = peopleCommandRepository;
        }
        public async Task<Guid> CreatePerson(CreatePersonRequestDto request)
        {
            var requestFroDb = _mapper.CreatePersonRequestModel(request);
            return await _peopleCommandRepository.CreatePerson(requestFroDb);

        }

        public async Task DeletePerson(DeletePersonRequestDto request)
        {
            var requestFroDb = _mapper.DeletePersonRequestModel(request);
            await _peopleCommandRepository.DeletePerson(requestFroDb);

        }

        public async Task<PersonResponseDto> GetById(Guid id)
        {
            var resultFromDb=await _peopleQueryRepository.GetById(id);
            return _mapper.PersonResponseDto(resultFromDb);

        }

        public async Task<SearchResponseModel<PeopleResponseDto>> GetList(SearchRequestModel<PeopleRequestDto, PeopleResponseDto> request)
        {
            var requestForDb = _mapper.SearchRequestModel_PeopleRequestModel_PeopleResponseModel(request);
            var resultFromDb = await _peopleQueryRepository.GetList(requestForDb);
            return _mapper.SearchResponseModel_PeopleResponseDto(resultFromDb);

        }

        public async Task UpdatePerson(UpdatePersonRequestDto request)
        {
            var requestFroDb = _mapper.UpdatePersonRequestModel(request);
            await _peopleCommandRepository.UpdatePerson(requestFroDb);
        }
    }
}

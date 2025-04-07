using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using CrudPeople.ApplicationService;
using PeopleGrpcService;
using Helpers.FilterSearch;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudPeople.grpc.Services
{
    public partial class PeopleGrpcService : PeopleService.PeopleServiceBase
    {
        private readonly IPeopleService _peopleService;
        private readonly PeopleGrpcServiceMapper _mapper;
        private readonly ILogger<PeopleGrpcService> _logger;

        public PeopleGrpcService(
            IPeopleService peopleService,
            ILogger<PeopleGrpcService> logger)
        {
            _peopleService = peopleService;
            _mapper = new PeopleGrpcServiceMapper();
            _logger = logger;
        }

        public override async Task<SearchPeopleResponse> GetPeopleList(
            SearchPeopleRequest request, ServerCallContext context)
        {
            var searchRequest = _mapper.SearchRequestModel_PeopleRequestDto_PeopleResponseDto(request); 

                var result = await _peopleService.GetList(searchRequest);
                return _mapper.SearchPeopleResponse(result);
           
        }

        public override async Task<PersonResponse> GetPersonById(
            PersonIdRequest request, ServerCallContext context)
        {
                var id = Guid.Parse(request.Id);
                var person = await _peopleService.GetById(id);
                return _mapper.PersonResponse(person);
           
        }

        public override async Task<PersonIdResponse> CreatePerson(
            CreatePersonRequest request, ServerCallContext context)
        {
                var createDto = _mapper.CreatePersonRequestDto(request);
                var id = await _peopleService.CreatePerson(createDto);
                return new PersonIdResponse { Id = id.ToString() };
            
        }

        public override async Task<Empty> UpdatePerson(
            UpdatePersonRequest request, ServerCallContext context)
        {
                var updateDto = _mapper.UpdatePersonRequestDto(request);
                await _peopleService.UpdatePerson(updateDto);
                return new Empty();
            
        }

        public override async Task<Empty> DeletePerson(
            DeletePersonRequest request, ServerCallContext context)
        {
                var deleteDto = new DeletePersonRequestDto
                {
                    Id = Guid.Parse(request.Id),
                    RowVersion = request.RowVersion.ToByteArray()
                };
                await _peopleService.DeletePerson(deleteDto);
                return new Empty();
            
        }
        
    }
}
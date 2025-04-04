using CrudPeople.API.ViewModels.Base;
using CrudPeople.API.ViewModels.Persons;
using CrudPeople.ApplicationService;
using ElasticLogger;
using Helpers.FilterSearch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CrudPeople.API.Controllers
{
    [LogCall]
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;
        private readonly PeopleControllerMapper _mapper;
        public PeopleController(IPeopleService peopleService)
        {
            _mapper = new PeopleControllerMapper();
            _peopleService = peopleService;
        }
        [Route("search/filter")]
        [HttpGet]
        [ProducesResponseType(type: typeof(SearchResponseModel<PersonViewModel>), statusCode: (int)HttpStatusCode.OK)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.InternalServerError)]
        public async Task<SearchResponseModel<PersonViewModel>> GetList([FromQuery] string selection, List<FilterModel> filters, List<OrderByModel> orderBies, List<string> groupBies, int page = 1, int size = 5)
        {
           var search= Helpers.FilterSearch.SearchRequestModel<PeopleRequestDto,PeopleResponseDto>.Create(selection, filters, orderBies, groupBies, page, size);
          var result= await _peopleService.GetList(search);
            return _mapper.SearchResponseModel_PersonViewModel(result);
        }
        [HttpGet]
        [ProducesResponseType(type: typeof(DataResponseViewModel<PersonDetailViewModel>), statusCode: (int)HttpStatusCode.Created)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.InternalServerError)]
        public async Task<PersonDetailViewModel> GetPerson(Guid id)
        {
            var result =await _peopleService.GetById(id);
            return _mapper.PersonDetailViewModel(result);
        }
        [HttpPost]
        [ProducesResponseType(type: typeof(Guid), statusCode: (int)HttpStatusCode.Created)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.InternalServerError)]
        public async Task<Guid> Create(PersonCreateViewModel request)
        {
            var requestForModel = _mapper.CreatePersonRequestDto(request);
           return await _peopleService.CreatePerson(requestForModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.InternalServerError)]
        public async Task Update([FromRoute]Guid id,PersonUpdateViewModel request)
        {
            var requestForModel = _mapper.UpdatePersonRequestDto(id,request);
             await _peopleService.UpdatePerson(requestForModel);

        }
        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(type: typeof(ResponseViewModel), statusCode: (int)HttpStatusCode.InternalServerError)]
        public async Task Delete([FromRoute] Guid id,PersonDeleteViewModel request)
        {
            var requestForModel = _mapper.DeletePersonRequestDto(id,request);
            await _peopleService.DeletePerson(requestForModel);

        }
    }
}

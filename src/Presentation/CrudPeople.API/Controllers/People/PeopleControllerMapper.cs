using CrudPeople.API.ViewModels.Persons;
using CrudPeople.ApplicationService;
using Helpers.FilterSearch;

namespace CrudPeople.API.Controllers
{
    class PeopleControllerMapper
    {
        internal CreatePersonRequestDto CreatePersonRequestDto(PersonCreateViewModel request)
        {
            return new CreatePersonRequestDto
            {
                BirthDate = request.BirthDate,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                PersonType=request.PersonType,
            };
        }

        internal DeletePersonRequestDto DeletePersonRequestDto(Guid id,PersonDeleteViewModel request)
        {
            return new DeletePersonRequestDto
            {
                RowVersion = request.RowVersion,
                Id = id,
            };
        }

        internal PersonDetailViewModel PersonDetailViewModel(PersonResponseDto result)
        {
            return new ViewModels.Persons.PersonDetailViewModel { 
            BirthDate = result.BirthDate,
            FirstName=result.FirstName,
            LastName=result.LastName,
            NationalCode=result.NationalCode,
            PersonTypeName=result.PersonTypeName,
            RowVersion=result.RowVersion
            };
        }

        internal SearchRequestModel<PeopleRequestDto, PeopleResponseDto> SearchRequestModel_PeopleRequestDto_PeopleResponseDto(SearchRequestModel<PersonViewModel> request)
        {
            return new SearchRequestModel<PeopleRequestDto, PeopleResponseDto>
            {
                Filter = request.Filter,
                GroupBies = request.GroupBies,
                OrderBies = request.OrderBies,
                Page = request.Page,
                Selections = request.Selections,
                Size = request.Size,
            };
        }

        internal SearchResponseModel<PersonViewModel> SearchResponseModel_PersonViewModel(SearchResponseModel<PeopleResponseDto> result)
        {
            return new SearchResponseModel<PersonViewModel>
            {
                Page = result.Page,
                Data = result.Data.Select(s => new PersonViewModel
                {
                    BirthDate = s.BirthDate,
                    FirstName = s.FirstName,
                    Id = s.Id,
                    LastName = s.LastName,
                    NationalCode = s.NationalCode,
                    PersonTypeName = s.PersonTypeName
                }).ToList(),
                TotalCount = result.TotalCount,
                TotalOfPages = result.TotalOfPages,
            };
            
        }

        internal UpdatePersonRequestDto UpdatePersonRequestDto(Guid id, PersonUpdateViewModel request)
        {
            return new UpdatePersonRequestDto
            {

                PersonType = request.PersonType,
                NationalCode=request.NationalCode,
                FirstName = request.FirstName,
                Id = id,    
                LastName = request.LastName,    
                BirthDate=request.BirthDate,
                RowVersion=request.RowVersion,
            };
        }
    }
}

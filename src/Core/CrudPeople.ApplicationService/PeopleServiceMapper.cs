using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using Helpers.FilterSearch;

namespace CrudPeople.ApplicationService
{
    class PeopleServiceMapper
    {
        public PeopleServiceMapper()
        {
        }

        internal CreatePersonRequestModel CreatePersonRequestModel(CreatePersonRequestDto request)
        {
            return new CreatePersonRequestModel
            {
                BirthDate = request.BirthDate,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                PersonTypeId = (byte)request.PersonType,
            };
        }

        internal DeletePersonRequestModel DeletePersonRequestModel(DeletePersonRequestDto request)
        {
            return new DeletePersonRequestModel
            {
                Id = request.Id,
                RowVersion = request.RowVersion,
            };
        }

        internal PersonResponseDto PersonResponseDto(PersonResponseModel resultFromDb)
        {
            return new PersonResponseDto
            {
                RowVersion = resultFromDb.RowVersion,
                Id = resultFromDb.Id,
                BirthDate = resultFromDb.BirthDate,
                FirstName = resultFromDb.FirstName,
                LastName = resultFromDb.LastName,
                NationalCode = resultFromDb.NationalCode,
                PersonTypeName = resultFromDb.PersonTypeName,
            };
        }

        internal SearchRequestModel<PeopleRequestModel, PeopleResponseModel> SearchRequestModel_PeopleRequestModel_PeopleResponseModel(SearchRequestModel<PeopleRequestDto, PeopleResponseDto> request)
        {
            return new SearchRequestModel<PeopleRequestModel, PeopleResponseModel>
            {
                Filter = request.Filter,
                GroupBies = request.GroupBies,
                OrderBies = request.OrderBies,
                Page = request.Page,
                Selections = request.Selections,
                Size = request.Size,
            };
        }

        internal SearchResponseModel<PeopleResponseDto> SearchResponseModel_PeopleResponseDto(SearchResponseModel<PeopleResponseModel> resultFromDb)
        {
            return new SearchResponseModel<PeopleResponseDto>
            {
                Page = resultFromDb.Page,
                Data = resultFromDb.Data.Select(s => new PeopleResponseDto
                {
                    BirthDate = s.BirthDate,
                    FirstName = s.FirstName,
                    Id = s.Id,
                    LastName = s.LastName,
                    NationalCode = s.NationalCode,
                    PersonTypeName = s.PersonTypeName,
                }).ToList(),
            };
        }

        internal UpdatePersonRequestModel UpdatePersonRequestModel(UpdatePersonRequestDto request)
        {
            return new UpdatePersonRequestModel
            {
                NationalCode = request.NationalCode,
                LastName = request.LastName,
                Id = request.Id,
                BirthDate = request.BirthDate,
                FirstName = request.FirstName,
                PersonTypeId = (byte)request.PersonType,
                RowVersion = request.RowVersion,
            };
        }
    }
}

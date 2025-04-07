using CrudPeople.ApplicationService;
using PeopleGrpcService;
using Helpers.FilterSearch;
using Helpers.Extentions;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace CrudPeople.grpc.Services
{
    public partial class PeopleGrpcService
    {
        class PeopleGrpcServiceMapper
        {
            internal CreatePersonRequestDto CreatePersonRequestDto(CreatePersonRequest request)
            {
                var ptype = PersonTypeConvert(request.PersonType);
                if(!ptype.HasValue)
                {
                    throw CrudPeople.CoreDomain.Helper.CrudPeopleException.Validation(" PersonType is not valid");
                }
                return new CreatePersonRequestDto
                {
                    PersonType = ptype.Value,
                    BirthDate = DateTimeExtentions.ConvertStringToDateTime(request.BirthDate),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    NationalCode = request.NationalCode,
                };
            }

            internal PersonResponse PersonResponse(PersonResponseDto person)
            {
                return new PersonResponse
                {
                    NationalCode = person.NationalCode,
                    LastName = person.LastName,
                    FirstName = person.FirstName,
                    BirthDate = person.BirthDate.ToShortDateString(),
                    Id = person.Id.ToString(),
                    PersonTypeName = person.PersonTypeName,
                    RowVersion = ByteString.CopyFrom(person.RowVersion),

                };
            }

            internal SearchPeopleResponse SearchPeopleResponse(SearchResponseModel<PeopleResponseDto> result)
            {
                var dt = (new RepeatedField<PersonResponse>());
                dt
                                .AddRange(
                        result.Data.Select(s2 => new PersonResponse
                        {
                            Id = s2.Id.ToString(),
                            FirstName = s2.FirstName,
                            LastName = s2.LastName,
                            BirthDate = s2.BirthDate.ToShortDateString(),
                            NationalCode = s2.NationalCode,
                            PersonTypeName = s2.PersonTypeName,

                        }));
               
               
                var response= new SearchPeopleResponse( )
                {
                    Page = result.Page,
                    TotalCount = result.TotalCount,
                    TotalOfPages = result.TotalOfPages,
                };
                response.Data.AddRange(dt);
                return response;
            }

            internal SearchRequestModel<PeopleRequestDto, PeopleResponseDto> SearchRequestModel_PeopleRequestDto_PeopleResponseDto(SearchPeopleRequest request)
            {
               return new SearchRequestModel<PeopleRequestDto, PeopleResponseDto>
                {
                    Page = request.Page,
                    Size = request.Size,
                    Filter = request.Filter,
                    OrderBies = request.OrderBies,
                    GroupBies = request.GroupBies,
                    Selections = request.Selections
                };
            }

            internal UpdatePersonRequestDto UpdatePersonRequestDto(UpdatePersonRequest request)
            {
                return new UpdatePersonRequestDto
                {
                    BirthDate = Helpers.Extentions.DateTimeExtentions.ConvertStringToDateTime( request.BirthDate),
                    FirstName = request.FirstName,
                    Id =Guid.Parse( request.Id),
                    LastName = request.LastName,
                    NationalCode = request.NationalCode,
                    PersonType = PersonTypeConvert(request.PersonType),
                    RowVersion = request.RowVersion.ToByteArray()
                };
            }
            private CoreDomain.Enums.PersonType? PersonTypeConvert(PersonType personType)
            {
                if(personType==PersonType.Legal)
                return (CoreDomain.Enums.PersonType.Legal);
                if (personType == PersonType.Individual)
                    return CoreDomain.Enums.PersonType.Individual;
                else
                    return null;
            }

        }

    }
}
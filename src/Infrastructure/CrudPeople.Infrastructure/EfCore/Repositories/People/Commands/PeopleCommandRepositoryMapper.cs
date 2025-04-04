using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Entities.People.Command;

namespace IDP.Infrastructure.Repositories.Attributes.Command
{
    internal class PeopleCommandRepositoryMapper
    {
        internal PeopleCommandEntity PeopleCommandEntity(CreatePersonRequestModel entity)
        {
            return CrudPeople.CoreDomain.Entities.People.Command.
                   PeopleCommandEntity.CreateModel(entity.DoingUserName,
                                                      firstName: entity.FirstName,
                                                      lastName: entity.LastName,
                                                      nationalCode: entity.NationalCode,
                                                      birthDate: entity.BirthDate,
                                                      personTypeId: entity.PersonTypeId);

        }

        internal PeopleCommandEntity PeopleCommandEntity(UpdatePersonRequestModel entity)
        {
            return CrudPeople.CoreDomain.Entities.People.Command.
            PeopleCommandEntity.UpdateModel(entity.Id,
                                                      entity.RowVersion,
                                                      entity.DoingUserName,
                                                      firstName: entity.FirstName,
                                                      lastName: entity.LastName,
                                                      nationalCode: entity.NationalCode,
                                                      birthDate: entity.BirthDate,
                                                      personTypeId: entity.PersonTypeId);
        }
    }
}

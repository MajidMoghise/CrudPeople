using CrudPeople.CoreDomain.Enums;
using CrudPeople.CoreDomain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Extentions;

namespace CrudPeople.CoreDomain.Entities.People.Command
{
    public class PeopleCommandEntity
    {
        private PeopleCommandEntity()
        {

        }
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string NationalCode { get; private set; }
        public DateTime BirthDate { get; private set; }
        public byte PersonTypeId { get; private set; }
        public byte[] RowVersion { get; private set; }
        public PersonType PersonType { get; private set; }
        public DateTime ChangeDate { get; private set; }
        public string DoingUserName { get; private set; }

        public static PeopleCommandEntity CreateModel(string doingUserName, string firstName, string lastName , string nationalCode , DateTime birthDate , byte personTypeId )
        {
            if (string.IsNullOrEmpty(doingUserName))
            {
                throw CrudPeopleException.Validation("doingUserName is null");
            }
            if (string.IsNullOrEmpty(nationalCode))
            {
                throw CrudPeopleException.Validation("nationalCode is null");
            }
            if (string.IsNullOrEmpty(firstName))
            {
                throw CrudPeopleException.Validation("firstName is null");
            }
            if (string.IsNullOrEmpty(lastName))
            {
                throw CrudPeopleException.Validation("lastName is null");
            }
            if (birthDate.Year <1900 && birthDate.Year>DateTime.Now.Year)
            {
                throw CrudPeopleException.Validation("birthDate is not correct ");
            }
            
            if (Enum.IsDefined(typeof(CrudPeople.CoreDomain.Enums.PersonType), personTypeId))
            {
                 var json = Helpers.Extentions.EnumExtensions.ToJson<CrudPeople.CoreDomain.Enums.PersonType>();
                throw CrudPeopleException.Validation("personType is not correct person type mustly "+json);
            }

            return new PeopleCommandEntity
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                NationalCode = nationalCode,
                BirthDate = birthDate,
                PersonTypeId = personTypeId,
                DoingUserName = doingUserName
            };
        }

        public static PeopleCommandEntity DeleteModel(Guid id, byte[] rowVersion, string doingUserName)
        {
            if (id == Guid.Empty)
            {
                throw CrudPeopleException.Validation("id is empty");
            }
            if (rowVersion is null)
            {
                throw CrudPeopleException.Validation("rowVersion is null");
            }
            if (string.IsNullOrEmpty(doingUserName))
            {
                throw CrudPeopleException.Validation("doingUserName is null");
            }
            return new PeopleCommandEntity
            {
                Id = id,
                RowVersion = rowVersion,
                DoingUserName = doingUserName
            };
        }

        public static PeopleCommandEntity UpdateModel(Guid id, byte[] rowVersion, string doingUserName, string firstName = null, string lastName = null, string nationalCode = null, DateTime? birthDate = null, byte? personTypeId = null)
        {
            if (id == Guid.Empty)
            {
                throw CrudPeopleException.Validation("id is empty");
            }
            if (rowVersion is null)
            {
                throw CrudPeopleException.Validation("rowVersion is null");
            }
            if (string.IsNullOrEmpty(doingUserName))
            {
                throw CrudPeopleException.Validation("doingUserName is null");
            }
            return new PeopleCommandEntity
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                NationalCode = nationalCode,
                BirthDate = birthDate.HasValue ? birthDate.Value : DateTime.MinValue,
                PersonTypeId = personTypeId.HasValue ? personTypeId.Value : byte.MinValue,
                RowVersion = rowVersion,
                DoingUserName = doingUserName
            };
        }
    }
}

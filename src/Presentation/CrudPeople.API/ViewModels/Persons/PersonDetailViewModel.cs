using CrudPeople.API.Attributes;

namespace CrudPeople.API.ViewModels.Persons
{
    [DangerousCharacter]
    public class PersonDetailViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string PersonTypeName { get; set; }
        public byte[] RowVersion{ get; set; }
    }    }

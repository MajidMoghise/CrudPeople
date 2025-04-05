using CrudPeople.API.Attributes;
using CrudPeople.CoreDomain.Enums;

namespace CrudPeople.API.ViewModels.Persons
{
    [DangerousCharacter]
    public class PersonCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public PersonType PersonType{ get; set; }
        public string NationalCode { get; set; }
    }
}

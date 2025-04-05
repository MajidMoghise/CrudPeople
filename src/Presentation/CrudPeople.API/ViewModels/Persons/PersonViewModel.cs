using CrudPeople.API.Attributes;

namespace CrudPeople.API.ViewModels.Persons
{
    [DangerousCharacter]
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string PersonTypeName { get; set; }
    }
}

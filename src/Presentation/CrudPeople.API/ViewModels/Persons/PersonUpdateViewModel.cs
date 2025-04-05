using CrudPeople.API.Attributes;
using CrudPeople.CoreDomain.Enums;

namespace CrudPeople.API.ViewModels.Persons
{
    [DangerousCharacter]
    public class PersonUpdateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCode { get; set; }
        public PersonType? PersonType { get; set; }
        public byte[] RowVersion{ get; set; }
    }}


namespace CrudPeople.CoreDomain.Contracts.People.Command.Models
{
    public class CreatePersonRequestModel
    {
        public string DoingUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public byte PersonTypeId { get; set; }
    }
}

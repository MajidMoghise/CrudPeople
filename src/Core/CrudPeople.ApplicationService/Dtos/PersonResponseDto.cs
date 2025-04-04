
namespace CrudPeople.ApplicationService
{
    public class PersonResponseDto
    {
        public byte[] RowVersion { get; set; }
        public string PersonTypeName { get; set; }
        public string NationalCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

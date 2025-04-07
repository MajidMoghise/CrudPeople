namespace CrudPeople.ApplicationService
{
    public class PeopleRequestDto
    {
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string PersonTypeName { get; set; }
    }
}

namespace CrudPeople.CoreDomain.Contracts.People.Query.Models
{
    public class PersonResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid Id { get; set; }
        public DateTime BirthDate { get; set; }
        public byte PersonTypeId { get; set; }
        public string PersonTypeName { get; set; }

    }
}

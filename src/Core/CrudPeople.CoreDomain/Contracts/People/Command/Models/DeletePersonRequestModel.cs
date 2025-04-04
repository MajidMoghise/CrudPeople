
namespace CrudPeople.CoreDomain.Contracts.People.Command.Models
{
    public class DeletePersonRequestModel
    {
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}

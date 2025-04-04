namespace CrudPeople.ApplicationService
{
    public class DeletePersonRequestDto
    {
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }

    }
}

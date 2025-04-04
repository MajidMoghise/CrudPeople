namespace CrudPeople.API.ViewModels.Persons
{
    public class PersonDeleteViewModel
    {
        public Guid Id { get; set; }
        public byte[] RowVersion{ get; set; }
    }
}

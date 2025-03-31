namespace CrudPeople.CoreDomain.Contracts.Base.Commands.Models
{
    public class DeleteRequestModel<Tid> 
    {
        private DeleteRequestModel()
        {
                
        }
        public Tid Id { get; private set; }
        public byte[] RowVersion { get; private set; }
        public string DoingUserName { get;private set; }
        public static DeleteRequestModel<Tid> Create(Tid id, string requestUserName, byte[] rowVersion)
        {
            return new DeleteRequestModel<Tid>
            {
                DoingUserName = requestUserName,
                Id = id,
                RowVersion = rowVersion
            };
        }
    }
}

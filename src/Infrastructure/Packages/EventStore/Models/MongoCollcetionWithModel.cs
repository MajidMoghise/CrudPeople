namespace EventStore.Models
{
    public class EventStoreCollcetionWithModel
    {
        public EventStoreCollcetionWithModel(string collectionName,EventStoreModel model)
        {
            CollectionName = collectionName;
            Model=model;
        }
        public string CollectionName { get;private set; }
        public EventStoreModel Model{ get;private set; }
    }

}
using EventStore.Models;

namespace EventStore
{
    public interface IEventStore
    {
        void Insert(string collectionName, EventStoreModel entity);
        Task InsertBulk(List<EventStoreCollcetionWithModel> entities);
        Task Insert(EventStoreCollcetionWithModel entity);
    }
}

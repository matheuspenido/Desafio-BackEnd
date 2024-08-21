namespace MyMessageContracts.SyncEntities.Consumers.Interfaces;

public interface ISyncEntityService<TEvent> where TEvent : class
{
    Task SyncEntityAsync(TEvent entityEvent);
}

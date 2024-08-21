using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.SyncEntities.Consumers;

public abstract class SyncEntityService<TBusEntity> : ISyncEntityService<CrudEntityEvent<TBusEntity>>
  where TBusEntity : class, IBusEntity
{
  public async Task SyncEntityAsync(CrudEntityEvent<TBusEntity> entityEvent)
  {
    switch (entityEvent.EventType)
    {
      case CrudEnum.Created:
        await CreateEntity(entityEvent.Entity);
        break;
      case CrudEnum.Updated:
        await UpdateEntity(entityEvent.Entity);
        break;
      case CrudEnum.Deleted:
        await RemoveEntity(entityEvent.Entity);
        break;
      default:
        break;
    }
  }

  protected abstract Task RemoveEntity(TBusEntity entity);
  protected abstract Task UpdateEntity(TBusEntity entity);
  protected abstract Task CreateEntity(TBusEntity entity);
}

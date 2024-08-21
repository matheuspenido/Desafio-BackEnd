using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.SyncEntities.Events.Base;

public abstract class BaseEvent<TBusEntity> : IEvent where TBusEntity : class, IBusEntity
{
  public Guid CorrelationId { get; }
  public DateTime Timestamp { get; }
  public TBusEntity Entity { get; }

  protected BaseEvent(TBusEntity entity)
  {
    CorrelationId = Guid.NewGuid();
    Timestamp = DateTime.UtcNow;
    Entity = entity;
  }

  protected BaseEvent(TBusEntity entity, Guid correlationId, DateTime timestamp)
  {
    CorrelationId = correlationId;
    Timestamp = timestamp;
    Entity = entity;
  }
}

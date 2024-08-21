using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.SyncEntities.Events.Base;

public abstract class BaseTypedEvent<TEvent, TBusEntity> : ITypedEvent<TEvent> where TEvent : Enum where TBusEntity : class, IBusEntity
{
    public Guid CorrelationId { get; }
    public DateTime Timestamp { get; }
    public TEvent EventType { get; }
    public TBusEntity Entity { get; }

    protected BaseTypedEvent(TEvent eventType, TBusEntity entity)
    {
        CorrelationId = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        EventType = eventType;
        Entity = entity;
    }

    protected BaseTypedEvent(TEvent eventType, TBusEntity entity, Guid correlationId, DateTime timestamp)
    {
        CorrelationId = correlationId;
        Timestamp = timestamp;
        EventType = eventType;
        Entity = entity;
    }
}
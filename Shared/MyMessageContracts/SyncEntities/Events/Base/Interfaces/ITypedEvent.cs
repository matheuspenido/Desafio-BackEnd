namespace MyMessageContracts.SyncEntities.Events.Base.Interfaces;

public interface ITypedEvent<TEvent> : IBaseEvent where TEvent : Enum
{
    TEvent EventType { get; }
}

namespace MyMessageContracts.SyncEntities.Events.Base.Interfaces;

public interface IBaseEvent
{
    public Guid CorrelationId { get; }
    public DateTime Timestamp { get; }
}

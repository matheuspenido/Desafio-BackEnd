using MyMessageContracts.SyncEntities.Events.Base;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.SyncEntities.Events;

public enum CrudEnum
{
    Created,
    Updated,
    Deleted
}

//Define the CRUD event between microservices.
//The CrudEntityEvent has a BusEntity (The DTO contract between Producer and Consumer) but in addition it has:
//Timestamp, EventType and CorrelationId.
//In this case, a CRUD event, it will define the events for: Created, Updated, Deleted.
public class CrudEntityEvent<TBusEntity> :
  BaseTypedEvent<CrudEnum, TBusEntity> where TBusEntity : class, IBusEntity
{
    public CrudEntityEvent(CrudEnum eventType, TBusEntity entity) : base(eventType, entity)
    {
    }
}
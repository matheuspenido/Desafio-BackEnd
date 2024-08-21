using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Contracts;

public class MotorcycleStatusBusEntity : IBusEntity
{
  public Guid Id { get; set; }
  public bool IsAvailable { get; set; }
}
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Contracts;

public class CustomerStatusBusEntity : IBusEntity
{
  public Guid Id { get; set; }
  public bool IsActive { get; set; }
}
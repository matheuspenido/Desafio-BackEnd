using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Contracts;

public class MotorcycleBusEntity : IBusEntity
{
  public Guid Id { get; set; }
  public string Model { get; set; } = string.Empty;
  public string LicensePlate { get; set; } = string.Empty;
  public int Year { get; set; }
  public bool IsAvailable { get; set; } = true;
}

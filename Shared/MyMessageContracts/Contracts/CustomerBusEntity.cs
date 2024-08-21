using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Contracts;

public enum DriverLicenseTypeEnum
{
  A,
  B,
  AB
}

public class CustomerBusEntity : IBusEntity
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string Cnpj { get; set; } = default!;
  public DateTime BirthDate { get; set; }
  public string DriverLicense { get; set; } = default!;
  public DriverLicenseTypeEnum DriverLicenseType { get; set; }
  public string? DriverLicenseImageLocation { get; set; }
  public bool IsActive { get; set; } = false;
}

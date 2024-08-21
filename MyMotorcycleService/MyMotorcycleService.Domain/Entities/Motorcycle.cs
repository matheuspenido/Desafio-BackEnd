using MyMotorcycleService.Domain.Entities.Interfaces;
using System.Text.RegularExpressions;

namespace MyMotorcycleService.Domain.Entities;

public class Motorcycle : IEntity
{
  public Guid Id { get; private set; }
  public string Model { get; private set; }
  public string LicensePlate { get; private set; }
  public int Year { get; private set; }
  public bool IsAvailable { get; private set; }

  public Motorcycle(Guid id, string model, string licensePlate, int year, bool isAvailable)
  {
    Id = id;
    Model = model;
    LicensePlate = licensePlate;
    Year = year;
    IsAvailable = isAvailable;
  }

  //For EF
  #nullable disable
  public Motorcycle()
  {
  }

  public void UpdateAvailableStatus(bool availability)
  {
    IsAvailable = availability;
  }

  public void UpdateLicensePlate(string licensePlate)
  {
    var normalizedLicensePlate = NormalizeLicensePlate(licensePlate);
    LicensePlate = normalizedLicensePlate;
  }

  public void MotorcycleUpdate(string model, string licensePlate, int year, bool isAvailable)
  {
    Model = model;
    LicensePlate = licensePlate;
    Year = year;
    IsAvailable = isAvailable;
  }
  private string NormalizeLicensePlate(string licensePlate)
  {
    return Regex.Replace(licensePlate, @"\s+", "").ToUpper();
  }
}

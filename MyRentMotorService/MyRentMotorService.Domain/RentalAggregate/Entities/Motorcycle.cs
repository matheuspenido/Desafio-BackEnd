using MyRentMotorService.Domain.RentalAggregate.Entities.Interfaces;

namespace MyRentMotorService.Domain.RentalAggregate.Entities;

public class Motorcycle : IEntity
{
  public Guid Id { get; private set; }
  public string Model { get; private set; }
  public string LicensePlate { get; private set; }
  public int Year { get; private set; }
  public bool IsAvailable { get; private set; } = true;

  public Motorcycle(Guid id, string model, string licensePlate, int year)
  {
    Id = id;
    Model = model;
    LicensePlate = licensePlate;
    Year = year;
  }

  //For EF
#nullable disable
  public Motorcycle()
  {

  }

  public void UpdateMotorcycle(Guid id, string model, string licensePlate, int year, bool isAvailable)
  {
    Id = id;
    Model = model;
    LicensePlate = licensePlate;
    Year = year;
    IsAvailable = isAvailable;
  }

  public void MarkAsRented()
  {
    IsAvailable = false;
  }

  public void MarkAsAvailable()
  {
    IsAvailable = true;
  }
}

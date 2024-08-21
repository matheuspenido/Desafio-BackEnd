using MyRentMotorService.Domain.RentalAggregate.Entities.Interfaces;

namespace MyRentMotorService.Domain.RentalAggregate.Entities;

public class MotorcyclesUnderAnalysis : IEntity
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public string Model { get; private set; } = default!;
  public string LicensePlate { get; private set; } = default!;
  public int Year { get; private set; } = default!;
  public DateTime RegisterDate { get; private set; } = default!;
  public Motorcycle Motorcycle { get; private set; } = default!;

  //For EF
  public MotorcyclesUnderAnalysis() { }

  public MotorcyclesUnderAnalysis(Motorcycle motorcycle, string model, string licensePlate, int year, DateTime registerDate)
  {
    Motorcycle = motorcycle;
    Model = model;
    LicensePlate = licensePlate;
    Year = year;
    RegisterDate = registerDate;
  }
}
using MyRentMotorService.Domain.RentalAggregate.Entities.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Enums;

namespace MyRentMotorService.Domain.RentalAggregate.Entities;

public class Customer : IEntity
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Cnpj { get; private set; }
  public DateTime BirthDate { get; private set; }
  public string DriverLicense { get; private set; }
  public DriverLicenseTypeEnum DriverLicenseType { get; private set; }
  public string? DriverLicenseImageLocation { get; private set; }
  public bool IsActive { get; private set; } = false;

  public Customer(
  Guid id,
  string name,
  string cnpj,
  DateTime birthDate,
  string driverLicense,
  DriverLicenseTypeEnum driverLicenseCategory,
  string? driverLicenseImageLocation)
  {
    Id = id;
    Name = name;
    Cnpj = cnpj;
    BirthDate = birthDate;
    DriverLicense = driverLicense;
    DriverLicenseType = driverLicenseCategory;
    DriverLicenseImageLocation = driverLicenseImageLocation;
    IsActive = false;
  }

  public void UpdateCustomer(
    Guid id,
    string name,
    string cnpj,
    DateTime birthDate,
    string driverLicense,
    DriverLicenseTypeEnum driverLicenseType,
    string? driverLicenseImageLocation,
    bool isActive
    )
  {
    Id = id;
    Name = name;
    Cnpj = cnpj;
    BirthDate = birthDate;
    DriverLicense = driverLicense;
    DriverLicenseType = driverLicenseType;
    DriverLicenseImageLocation = driverLicenseImageLocation;
    IsActive = isActive;
  }

  public void MarkAsActiveCustomer()
  {
    IsActive = true;
  }

  public void MarkAsInactive()
  {
    IsActive = false;
  }

  //For EF
#nullable disable
  public Customer()
  {
  }
}

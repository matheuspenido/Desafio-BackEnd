using MyCustomerService.Domain.Entities.Enums;

namespace MyCustomerService.Domain.Entities;

public class Customer
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Cnpj { get; private set; }
  public DateTime BirthDate { get; private set; }
  public string DriverLicense { get; private set; }
  public DriverLicenseTypeEnum DriverLicenseType { get; private set; }
  public string? DriverLicenseImageFileName { get; private set; }
  public string? DriverLicenseImageLocation { get; private set; }
  public string? DriverLicenseImageContentType { get; private set; }
  public bool ActiveCustomer { get; private set; }

  public Customer(
    Guid id,
    string name,
    string cnpj,
    DateTime birthDate,
    string driverLicense,
    DriverLicenseTypeEnum driverLicenseType,
    bool activeCustomer)
  {
    Id = id;
    Name = name;
    Cnpj = cnpj;
    BirthDate = birthDate;
    DriverLicense = driverLicense;
    DriverLicenseType = driverLicenseType;
    ActiveCustomer = activeCustomer;
  }

  //For EF
#nullable disable
  public Customer()
  {
  }

  public void UpdateActiveStatus(bool active)
  {
    ActiveCustomer = active;
  }

  public void RemoveDriverLicenseImage()
  {
    DriverLicenseImageFileName = null;
    DriverLicenseImageContentType = null;
    DriverLicenseImageLocation = null;
  }

  public void UpdateDriverLicenseImage(string fileName, string contentType, string location)
  {
    DriverLicenseImageFileName = fileName;
    DriverLicenseImageContentType = contentType;
    DriverLicenseImageLocation = location;
  }

  public void CustomerUpdate(
    string name,
    string cnpj,
    DateTime birthDate,
    string driverLicense,
    DriverLicenseTypeEnum driverLicenseType)
  {
    Name = name;
    Cnpj = cnpj;
    BirthDate = birthDate;
    DriverLicense = driverLicense;
    DriverLicenseType = driverLicenseType;
  }
}

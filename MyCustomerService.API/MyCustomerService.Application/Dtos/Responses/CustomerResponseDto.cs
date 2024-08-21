using MyCustomerService.Domain.Entities.Enums;

namespace MyCustomerService.Application.Dtos.Responses;

public class CustomerResponseDto
{
  public string? Name { get; set; } = null!;
  public string? Cnpj { get; set; } = null!;
  public DateTime? BirthDate { get; set; }
  public string? DriverLicense { get; set; } = null!;
  public DriverLicenseTypeEnum? DriverLicenseType { get; set; }
  public string? DriverLicenseImageFileName { get; set; }
  public string? DriverLicenseImageLocation { get; set; }
  public string? DriverLicenseImageContentType { get; set; }
  public bool ActiveCustomer { get; set; }
}

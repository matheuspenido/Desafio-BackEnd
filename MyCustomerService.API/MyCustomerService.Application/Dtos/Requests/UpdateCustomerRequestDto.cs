using MyCustomerService.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyCustomerService.Application.Dtos.Requests;

public class UpdateCustomerRequestDto
{
  [Required(ErrorMessage = "Name is required")]
  public string Name { get; set; } = null!;

  [Required(ErrorMessage = "CNPJ is required")]
  public string Cnpj { get; set; } = null!;
  public DateTime BirthDate { get; set; }

  [Required(ErrorMessage = "Driver License is required")]
  public string DriverLicense { get; set; } = null!;

  [Required(ErrorMessage = "Driver License Type is required")]
  public DriverLicenseTypeEnum DriverLicenseType { get; set; }
}

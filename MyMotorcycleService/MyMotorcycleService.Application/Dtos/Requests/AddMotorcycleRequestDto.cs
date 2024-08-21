using System.ComponentModel.DataAnnotations;

namespace MyMotorcycleService.Application.Dtos.Requests;

public class AddMotorcycleRequestDto
{
  [Required(ErrorMessage = "Model is required")]
  public string Model { get; set; } = null!;

  [Required(ErrorMessage = "License Plate is required")]
  public string LicensePlate { get; set; } = null!;

  [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
  public int Year { get; set; }
}

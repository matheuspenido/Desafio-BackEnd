namespace MyRentMotorService.Application.Dtos.Responses;

public class ResponseGetCostPreviewApplicationDto
{
  public string Model { get; set; } = default!;
  public string LicensePlate { get; set; } = default!;
  public string? CustomerName { get; set; }
  public string? DriverLicense { get; set; }
  public DateTime RentalDate { get; set; }
  public DateTime EstimatedReturnDate { get; set; }
  public decimal EstimatedPrice { get; set; }
}

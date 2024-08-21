using MyRentMotorService.API.Dtos.Enums;

namespace MyRentMotorService.API.Dtos;

public class RequestCreateRentalDto
{
  public string LicensePlate { get; set; } = default!;
  public string DriverLicense { get; set; } = default!;
  public DateTime StartDate { get; set; }
  public RequestRentalPlanEnum RentalPlan { get; set; }
}

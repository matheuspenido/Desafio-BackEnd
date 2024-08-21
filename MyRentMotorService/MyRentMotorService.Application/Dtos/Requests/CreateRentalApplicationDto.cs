using MyRentMotorService.Application.Dtos.Enums;

namespace MyRentMotorService.Application.Dtos.Requests;

public class CreateRentalApplicationDto
{
    public string LicensePlate { get; set; } = default!;
    public string DriverLicense { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public RentalPlanEnumApplicationDto RentalPlan { get; set; }
}

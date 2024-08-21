namespace MyRentMotorService.Application.Dto;

public class RequestPreviewInfoApplicationDto
{
  public string DriverLicense { get; set; } = null!;
  public DateTime ReturnDate { get; set; }
}
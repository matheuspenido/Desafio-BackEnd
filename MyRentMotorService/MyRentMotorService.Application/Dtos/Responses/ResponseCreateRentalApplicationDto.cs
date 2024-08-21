namespace MyRentMotorService.Application.Dtos.Responses;

public class ResponseCreateRentalApplicationDto
{
  public Guid Id { get; set; }
  public Guid MotorcycleId { get; set; }
  public string LicensePlate { get; set; } = default!;
  public Guid CustomerId { get; set; }
  public string? CustomerName { get; set; }
  public string? DriverLicense { get; set; }
  public DateTime RentalDate { get; set; }
  public DateTime? EstimatedReturnDate { get; set; }
  public decimal EstimatedPrice { get; set; }
}

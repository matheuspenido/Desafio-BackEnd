namespace MyRentMotorService.Application.Dto;

public class RequestCompleteRentalApplicationDto
{
  public Guid Id { get; set; }
  public DateTime ReturnDate { get; set; }
}
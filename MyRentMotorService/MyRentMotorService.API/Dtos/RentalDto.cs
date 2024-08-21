namespace MyRentMotorService.API.Dtos;

public class RentalDto
{
  public Guid Id { get; set; }
  public Guid MotorcycleId { get; set; }
  public Guid CustomerId { get; set; }
  public DateTime RentalDate { get; set; }
  public DateTime? EstimatedReturnDate { get; set; }
  public DateTime? ReturnDate { get; set; }
  public decimal TotalPrice { get; set; }
}


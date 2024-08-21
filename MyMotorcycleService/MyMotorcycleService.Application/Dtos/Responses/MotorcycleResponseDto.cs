namespace MyMotorcycleService.Application.Dtos.Responses;

public class MotorcycleResponseDto
{
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public int Year { get; set; }
    public bool IsAvailable { get; set; } = true;
}

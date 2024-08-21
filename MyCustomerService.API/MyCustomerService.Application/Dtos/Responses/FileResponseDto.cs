namespace MyCustomerService.Application.Dtos.Responses;

public class FileResponseDto
{
  public Stream FileContent { get; set; } = null!;
  public string FileName { get; set; } = null!;
  public string ContentType { get; set; } = null!;
}

using Microsoft.AspNetCore.Http;

namespace MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;

public interface IFileRepository
{
  Task<string> UploadFileAsync(IFormFile file, string destinationPath);
  Task<Stream> DownloadFileAsync(string filePath);
  Task DeleteFileAsync(string filePath);
  Task<IEnumerable<string>> ListFileAsync(string directoryPath);

}

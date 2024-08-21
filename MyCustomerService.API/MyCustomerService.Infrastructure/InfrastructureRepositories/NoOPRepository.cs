using Microsoft.AspNetCore.Http;
using MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;

namespace MyCustomerService.Infrastructure.InfrastructureRepositories;

public class NoOPRepository : IFileRepository
{
  public Task DeleteFileAsync(string filePath)
  {
    Console.WriteLine("AWS S3 is not configured. No file operations will be performed.");
    return Task.CompletedTask;
  }

  public Task<Stream> DownloadFileAsync(string filePath)
  {
    Console.WriteLine("AWS S3 is not configured. No file operations will be performed.");
    return Task.FromResult<Stream>(null);
  }

  public Task<IEnumerable<string>> ListFileAsync(string directoryPath)
  {
    Console.WriteLine("AWS S3 is not configured. No file operations will be performed.");
    return Task.FromResult<IEnumerable<string>>(null);
  }

  public Task<string> UploadFileAsync(IFormFile file, string destinationPath)
  {
    Console.WriteLine("AWS S3 is not configured. No file operations will be performed.");
    return Task.FromResult<string>(null);
  }
}

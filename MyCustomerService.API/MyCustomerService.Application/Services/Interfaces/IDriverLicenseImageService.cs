using Microsoft.AspNetCore.Http;
using MyCustomerService.Application.Dtos.Responses;

namespace MyCustomerService.Application.Services.Interfaces;

public interface IDriverLicenseImageService
{
  Task<FileResponseDto?> DownloadDriverLicenseImageAsync(string driverLicense);
  Task<string> UploadDriverLicenseImageAsync(string driverLicense, IFormFile formFile);
  Task RemoveDriverLicenseImageAsync(string driverLicense);
}

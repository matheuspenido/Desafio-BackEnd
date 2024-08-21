using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyCustomerService.Application.Dtos.Responses;
using MyCustomerService.Application.Services.Interfaces;
using MyCustomerService.Domain.Entities;
using MyCustomerService.Infrastructure.Database.EF.Interfaces;
using MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;
using MyCustomerService.Infrastructure.Messaging.Extensions;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using System.Text.RegularExpressions;

namespace MyCustomerService.Application.Services;

public class DriverLicenseImageService : IDriverLicenseImageService
{
  private readonly IEntityFrameworkRepository<Customer> _customerRepository;
  private readonly IEventPublisher<CrudEntityEvent<CustomerBusEntity>> _customerEventPublish;
  private readonly IFileRepository _fileRepository;

  public DriverLicenseImageService(IEntityFrameworkRepository<Customer> customerService, IEventPublisher<CrudEntityEvent<CustomerBusEntity>> customerEventPublish, IFileRepository fileRepository)
  {
    _customerRepository = customerService;
    _customerEventPublish = customerEventPublish;
    _fileRepository = fileRepository;
  }

  public async Task<FileResponseDto?> DownloadDriverLicenseImageAsync(string driverLicense)
  {
    var customer = await FindCustomerByDriverLicenseAsync(driverLicense);

    if (customer is null || string.IsNullOrEmpty(customer.DriverLicenseImageFileName))
      return null;

    var fileStream = await _fileRepository.DownloadFileAsync(customer.DriverLicenseImageFileName);

    if (fileStream == null)
      return null;

    var response = new FileResponseDto
    {
      ContentType = customer.DriverLicenseImageContentType ?? "unknow",
      FileName = customer.DriverLicenseImageFileName ?? "unknow",
      FileContent = fileStream
    };

    return response;
  }

  public async Task RemoveDriverLicenseImageAsync(string driverLicense)
  {
    var customer = await FindCustomerByDriverLicenseAsync(driverLicense);
    var fileName = customer?.DriverLicenseImageFileName;

    if (customer is not null && fileName is not null)
    {
      await _fileRepository.DeleteFileAsync(fileName);
      customer.RemoveDriverLicenseImage();
      await _customerRepository.SaveChangesAsync();

      var customerEvent = CreateEvent(customer, CrudEnum.Updated);
      await _customerEventPublish.PublishAsync(customerEvent);
    }
  }

  public async Task<string> UploadDriverLicenseImageAsync(string driverLicense, IFormFile formFile)
  {
    ValidateFile(formFile);

    var customer = await FindCustomerByDriverLicenseAsync(driverLicense);
    var url = string.Empty;

    if (customer is not null)
    {
      var newFileName = GetFileName(driverLicense, formFile);
      url = await _fileRepository.UploadFileAsync(formFile, newFileName);
      customer.UpdateDriverLicenseImage(newFileName, formFile.ContentType, url);
      await _customerRepository.SaveChangesAsync();

      var customerEvent = CreateEvent(customer, CrudEnum.Updated);
      await _customerEventPublish.PublishAsync(customerEvent);
    }

    return url;
  }

  //To be moved to FluentValidation in the future.
  private void ValidateFile(IFormFile formFile)
  {
    ArgumentNullException.ThrowIfNull(formFile);

    var fileExtension = Path.GetExtension(formFile.FileName);
    var fileMimeType = formFile.ContentType;

    //To move to FluentValidation in the future.
    if (string.IsNullOrEmpty(fileMimeType) || (fileMimeType != "image/jpeg" && fileMimeType != "image/png"))
      throw new ArgumentException(nameof(fileMimeType));

    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
      throw new ArgumentException(nameof(fileExtension));
  }

  private string GetFileName(string driverLicense, IFormFile formFile)
  {
    return $"{driverLicense}-{formFile.FileName}";
  }

  private async Task<Customer?> FindCustomerByDriverLicenseAsync(string driverLicense)
  {
    var normalizedDriverLicense = NormalizedIdentifiers(driverLicense);

    var customer = await _customerRepository.GetQueryable()
      .SingleOrDefaultAsync(r => r.DriverLicense == normalizedDriverLicense);

    return customer;
  }

  private string NormalizedIdentifiers(string driverLicense)
  {
    return Regex.Replace(driverLicense, @"\s+", "").ToUpper();
  }

  private CrudEntityEvent<CustomerBusEntity> CreateEvent(Customer customer, CrudEnum eventType)
  {
    var customerBusEntity = new CustomerBusEntity
    {
      Id = customer.Id,
      Name = customer.Name,
      BirthDate = customer.BirthDate,
      Cnpj = customer.Cnpj,
      DriverLicense = customer.DriverLicense,
      DriverLicenseImageLocation = customer.DriverLicenseImageLocation,
      DriverLicenseType = customer.DriverLicenseType.ToEventEnum()
    };

    var customerEvent = new CrudEntityEvent<CustomerBusEntity>(eventType, customerBusEntity);
    return customerEvent;
  }
}

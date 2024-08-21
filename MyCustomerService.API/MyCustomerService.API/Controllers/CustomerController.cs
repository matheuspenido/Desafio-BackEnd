using Microsoft.AspNetCore.Mvc;
using MyCustomerService.Application.Dtos.Requests;
using MyCustomerService.Application.Dtos.Responses;
using MyCustomerService.Application.Services.Interfaces;

namespace MyCustomerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
  private readonly IEntityRepository _customerService;
  private readonly IDriverLicenseImageService _driverLicenseImageService;

  public CustomerController(IEntityRepository customerService, IDriverLicenseImageService driverLicenseImageService)
  {
    _customerService = customerService;
    _driverLicenseImageService = driverLicenseImageService;
  }

  [HttpGet("{driverLicense}")]
  public async Task<ActionResult<CustomerResponseDto>> GetCustomerByDriverLicense(string driverLicense)
  {
    var customer = await _customerService.GetCustomerByDriverLicense(driverLicense);
    if (customer == null)
    {
      return NotFound();
    }
    return Ok(customer);
  }

  [HttpGet("cnpj/{cnpj}")]
  public async Task<ActionResult<CustomerResponseDto>> GetCustomerByCnpj(string cnpj)
  {
    var customer = await _customerService.GetCustomerByCnpj(cnpj);
    if (customer == null)
    {
      return NotFound();
    }
    return Ok(customer);
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomers()
  {
    var customers = await _customerService.GetAllCustomers();
    return Ok(customers);
  }

  [HttpPost]
  public async Task<ActionResult> AddCustomer([FromBody] AddCustomerRequestDto customer)
  {
    await _customerService.AddCustomer(customer);
    return CreatedAtAction(nameof(GetCustomerByDriverLicense), new { driverLicense = customer.DriverLicense }, customer);
  }

  [HttpPut("cnpj/{cnpj}")]
  public async Task<ActionResult> UpdateCustomerByCnpj(string cnpj, [FromBody] UpdateCustomerRequestDto customer)
  {
    if (cnpj != customer.Cnpj)
    {
      return BadRequest("Cnpj in URL does not match Cnpj in body");
    }

    await _customerService.UpdateCustomer(customer);
    return NoContent();
  }

  [HttpPut("{driverLicense}")]
  public async Task<ActionResult> UpdateCustomerByDriverLicense(string driverLicense, [FromBody] UpdateCustomerRequestDto customer)
  {
    if (driverLicense != customer.DriverLicense)
      return BadRequest("Driver License in URL does not match Drivr License in body");
    
    await _customerService.UpdateCustomer(customer);

    return NoContent();
  }

  [HttpDelete("driverlicense/{driverLicense}")]
  public async Task<ActionResult> RemoveCustomerByDriverLicense(string driverLicense)
  {
    await _customerService.RemoveCustomerByDriverLicense(driverLicense);

    return NoContent();
  }

  [HttpDelete("{cnpj}")]
  public async Task<ActionResult> RemoveCustomerByCnpjLicense(string cnpj)
  {
    await _customerService.RemoveCustomerByCnpj(cnpj);

    return NoContent();
  }

  [HttpDelete("driverlicense/{driverLicense}/driverlicenseimage")]
  public async Task<ActionResult> RemoveDriverLicenseImage(string driverLicense)
  {
    await _driverLicenseImageService.RemoveDriverLicenseImageAsync(driverLicense);

    return NoContent();
  }

  [HttpPost("driverlicense/{driverLicense}/driverlicenseimage")]
  public async Task<ActionResult> UploadDriverLicenseImage([FromRoute]string driverLicense, [FromForm] IFormFile file)
  {
    var url = await _driverLicenseImageService.UploadDriverLicenseImageAsync(driverLicense, file);

    return Ok(new { Url = url });
  }

  [HttpGet("driverlicense/{driverLicense}/driverlicenseimage")]
  public async Task<ActionResult> DownloadDriverLicenseImage(string driverLicense)
  {
    var fileResponse = await _driverLicenseImageService.DownloadDriverLicenseImageAsync(driverLicense);

    if (fileResponse == null)
      return NotFound();

    return File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName);
  }
}

using Microsoft.EntityFrameworkCore;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentMotorService.Application.Services.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentMotorService.Application.Services;

public class CustomerService : ICustomerService
{
  private readonly IAppDbContext _context;

  public CustomerService(IAppDbContext context)
  {
    _context = context;
  }

  public async Task<Customer> GetCustomerByIdAsync(Guid id)
  {
    var result = await _context.Set<Customer>().FindAsync(id);

    if (result is null)
      throw new Exception($"The customer for {id} doesnt exist.");

    return result;
  }

  public async Task<Customer> GetCustomerByLicenseDriverAsync(string driverLicense)
  {
    var result = await _context.Set<Customer>().SingleOrDefaultAsync(c => c.DriverLicense == driverLicense.ToUpper());
    
    if (result is null)
      throw new Exception($"The customer for driver license {driverLicense} doesnt exist.");

    return result;
  }
}

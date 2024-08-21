using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentMotorService.Application.Services.Interfaces;

public interface ICustomerService
{
  Task<Customer> GetCustomerByIdAsync(Guid customerId);
  Task<Customer> GetCustomerByLicenseDriverAsync(string driverLicense);
}

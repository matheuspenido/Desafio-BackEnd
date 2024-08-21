using MyCustomerService.Application.Dtos.Requests;
using MyCustomerService.Application.Dtos.Responses;

namespace MyCustomerService.Application.Services.Interfaces;

public interface IEntityRepository
{
  Task<CustomerResponseDto?> GetCustomerByCnpj(string cnpj);
  Task<CustomerResponseDto?> GetCustomerByDriverLicense(string driverLicense);
  Task AddCustomer(AddCustomerRequestDto customer);
  Task UpdateCustomer(UpdateCustomerRequestDto customer);
  Task RemoveCustomerByDriverLicense(string driverLicense);
  Task RemoveCustomerByCnpj(string cnpj);
  Task<IEnumerable<CustomerResponseDto>?> GetAllCustomers();
}

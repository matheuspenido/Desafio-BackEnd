using MyCustomerService.Domain.Entities;

namespace MyCustomerService.Domain.Interfaces;

public interface ICustomerRepository
{
  Task<Customer?> GetByDriverLicenseAsync(string driverLicense);
  Task<Customer?> GetByCnpjAsync(string cnpj);
  Task<Customer?> GetByIdAsync(Guid id);
  Task<IEnumerable<Customer>?> GetAllAsync();
  Task AddAsync(Customer customer);
  void Update(Customer customer);
  Task<Customer?> RemoveByDriverLicenseAsync(string driverLicense);
  Task<Customer?> RemoveByCnpj(string cnpj);
  Task<Customer?> RemoveById(Guid id);
}

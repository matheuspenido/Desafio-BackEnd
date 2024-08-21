using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers;
using MyRentalMotorService.Infrastructure.Messaging.Extensions;
using MyRentMotorService.Domain.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentalMotorService.Infrastructure.Services;

//The sync service will be used by the Consumer to define the business rule to Create, Update or Delete
//the entity on the Consumer. For Customer in this case.
public class CustomerCrudBus : SyncEntityService<CustomerBusEntity>
{
  private readonly IEntityRepository<Customer> _customerRepository;

  public CustomerCrudBus(IEntityRepository<Customer> customerRepository)
  {
    _customerRepository = customerRepository;
  }

  protected override async Task CreateEntity(CustomerBusEntity entity)
  {
    var customer = ConvertToCustomer(entity);
    await _customerRepository.AddAsync(customer);
    await _customerRepository.SaveChangesAsync();
  }

  protected override async Task RemoveEntity(CustomerBusEntity entity)
  {
    var customer = ConvertToCustomer(entity);
    _customerRepository.Remove(customer);
    await _customerRepository.SaveChangesAsync();
  }

  protected override async Task UpdateEntity(CustomerBusEntity entity)
  {
    var customer = ConvertToCustomer(entity);
    _customerRepository.Update(customer);
    await _customerRepository.SaveChangesAsync();
  }

  private Customer ConvertToCustomer(CustomerBusEntity customerBusEntity)
  {
    return new Customer(
      customerBusEntity.Id,
      customerBusEntity.Name,
      customerBusEntity.Cnpj,
      customerBusEntity.BirthDate,
      customerBusEntity.DriverLicense,
      customerBusEntity.DriverLicenseType.ToEntityEnum(),
      customerBusEntity.DriverLicenseImageLocation);
  }
}

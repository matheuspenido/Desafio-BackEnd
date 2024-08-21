using MyCustomerService.Domain.Entities;
using MyCustomerService.Infrastructure.Database.EF.Interfaces;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers;

namespace MyCustomerService.Infrastructure.Services;

public class CustomerCrudBus : SyncEntityService<CustomerStatusBusEntity>
{
  private readonly IEntityFrameworkRepository<Customer> _customerRepository;

  public CustomerCrudBus(IEntityFrameworkRepository<Customer> customerRepository)
  {
    _customerRepository = customerRepository;
  }

  protected override Task CreateEntity(CustomerStatusBusEntity entity)
  {
    throw new NotImplementedException();
  }

  protected override Task RemoveEntity(CustomerStatusBusEntity entity)
  {
    throw new NotImplementedException();
  }

  protected async override Task UpdateEntity(CustomerStatusBusEntity entity)
  {
    var customer = await _customerRepository.GetByIdAsync(entity.Id);
    
    if (customer is not null)
      customer.UpdateActiveStatus(entity.IsActive);

    await _customerRepository.SaveChangesAsync();
  }
}

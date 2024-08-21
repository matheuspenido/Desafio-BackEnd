using MassTransit;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;

namespace MyCustomerService.Infrastructure.Messaging.RabbitMQ.Consumers;

public class RentalStatusCustomerConsumer : IConsumer<CrudEntityEvent<CustomerStatusBusEntity>>
{
  private readonly ISyncEntityService<CrudEntityEvent<CustomerStatusBusEntity>> _syncEntityService;

  public RentalStatusCustomerConsumer(ISyncEntityService<CrudEntityEvent<CustomerStatusBusEntity>> syncEntityService)
  {
    _syncEntityService = syncEntityService;
  }

  public async Task Consume(ConsumeContext<CrudEntityEvent<CustomerStatusBusEntity>> context)
  {
    await _syncEntityService.SyncEntityAsync(context.Message);
  }
}

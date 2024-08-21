using MassTransit;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;

namespace MyRentalMotorService.Infrastructure.Messaging.RabbitMQ.Consumers;

public class CustomerConsumer : IConsumer<CrudEntityEvent<CustomerBusEntity>>
{
  private readonly ISyncEntityService<CrudEntityEvent<CustomerBusEntity>> _syncEntityService;

  public CustomerConsumer(ISyncEntityService<CrudEntityEvent<CustomerBusEntity>> syncEntityService)
  {
    _syncEntityService = syncEntityService;
  }

  public async Task Consume(ConsumeContext<CrudEntityEvent<CustomerBusEntity>> context)
  {
    await _syncEntityService.SyncEntityAsync(context.Message);
  }
}

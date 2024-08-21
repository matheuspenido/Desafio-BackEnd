using MassTransit;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;

namespace MyRentalMotorService.Infrastructure.Messaging.RabbitMQ.Consumers;

public class MotorcycleConsumer : IConsumer<CrudEntityEvent<MotorcycleBusEntity>>
{
  private readonly ISyncEntityService<CrudEntityEvent<MotorcycleBusEntity>> _syncEntityService;
  public MotorcycleConsumer(ISyncEntityService<CrudEntityEvent<MotorcycleBusEntity>> syncEntityService)
  {
    _syncEntityService = syncEntityService;
  }

  public async Task Consume(ConsumeContext<CrudEntityEvent<MotorcycleBusEntity>> context)
  {
    await _syncEntityService.SyncEntityAsync(context.Message);
  }
}

using MassTransit;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;

namespace MyMotorcycleService.Infrastructure.Messaging.RabbitMQ.Consumers;

public class RentalStatusMotorcycleConsumer : IConsumer<CrudEntityEvent<MotorcycleStatusBusEntity>>
{
  private readonly ISyncEntityService<CrudEntityEvent<MotorcycleStatusBusEntity>> _syncEntityService;

  public RentalStatusMotorcycleConsumer(ISyncEntityService<CrudEntityEvent<MotorcycleStatusBusEntity>> syncEntityService)
  {
    _syncEntityService = syncEntityService;
  }

  public async Task Consume(ConsumeContext<CrudEntityEvent<MotorcycleStatusBusEntity>> context)
  {
    await _syncEntityService.SyncEntityAsync(context.Message);
  }
}

using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers;
using MyMotorcycleService.Domain.Entities;
using MyMotorcycleService.Infrastructure.Database.EF.Interfaces;

namespace MyMotorcycleService.Infrastructure.Services;

public class MotorcycleCrudBus : SyncEntityService<MotorcycleStatusBusEntity>
{
  private readonly IEntityFrameworkRepository<Motorcycle> _motorcycleRepository;

  public MotorcycleCrudBus(IEntityFrameworkRepository<Motorcycle> motorcycleRepository)
  {
    _motorcycleRepository = motorcycleRepository;
  }

  protected override Task CreateEntity(MotorcycleStatusBusEntity entity)
  {
    throw new NotImplementedException();
  }

  protected override Task RemoveEntity(MotorcycleStatusBusEntity entity)
  {
    throw new NotImplementedException();
  }

  protected async override Task UpdateEntity(MotorcycleStatusBusEntity entity)
  {
    var motorcycle = await _motorcycleRepository.GetByIdAsync(entity.Id);

    if (motorcycle is not null)
      motorcycle.UpdateAvailableStatus(entity.IsAvailable);

    await _motorcycleRepository.SaveChangesAsync();
  }
}

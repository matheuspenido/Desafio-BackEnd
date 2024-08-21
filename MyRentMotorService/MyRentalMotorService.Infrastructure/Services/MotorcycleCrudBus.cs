using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Consumers;
using MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork.Interfaces;
using MyRentMotorService.Domain.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentalMotorService.Infrastructure.Services;

public class MotorcycleCrudBus : SyncEntityService<MotorcycleBusEntity>
{
  private readonly IEntityRepository<Motorcycle> _motorcycleRepository;
  private readonly IEntityRepository<MotorcyclesUnderAnalysis> _motorcycleUnderAnalysisRepository;
  private readonly IUnitOfWork _unitOfWork;

  public MotorcycleCrudBus(
    IEntityRepository<Motorcycle> motorcycleRepository,
    IEntityRepository<MotorcyclesUnderAnalysis> motorcycleUnderAnalysisRepository,
    IUnitOfWork unitOfWork)
  {
    _motorcycleRepository = motorcycleRepository;
    _motorcycleUnderAnalysisRepository = motorcycleUnderAnalysisRepository;
    _unitOfWork = unitOfWork;
  }

  protected async override Task CreateEntity(MotorcycleBusEntity entity)
  {
    var motorcycle = ConvertToMotorcycle(entity);
    var motorcycleUnderAnalysis = ConvertToMotorcycleUnderAnalisys(motorcycle);

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      await _motorcycleRepository.AddAsync(motorcycle);

      if (motorcycleUnderAnalysis.Year == 2024)
        await _motorcycleUnderAnalysisRepository.AddAsync(motorcycleUnderAnalysis);

      await _unitOfWork.CommitAsync();
    }
    catch
    {
      await _unitOfWork.RollbackAsync();
      throw;
    }
  }

  protected async override Task RemoveEntity(MotorcycleBusEntity entity)
  {
    var motorcycle = ConvertToMotorcycle(entity);
    _motorcycleRepository.Remove(motorcycle);
    await _motorcycleRepository.SaveChangesAsync();
  }

  protected async override Task UpdateEntity(MotorcycleBusEntity entity)
  {
    var motorcycle = ConvertToMotorcycle(entity);
    _motorcycleRepository.Update(motorcycle);
    await _motorcycleRepository.SaveChangesAsync();
  }

  private Motorcycle ConvertToMotorcycle(MotorcycleBusEntity motorcycleBusEntity)
  {
    return new Motorcycle(
      motorcycleBusEntity.Id,
      motorcycleBusEntity.Model,
      motorcycleBusEntity.LicensePlate,
      motorcycleBusEntity.Year);
  }
  private MotorcyclesUnderAnalysis ConvertToMotorcycleUnderAnalisys(Motorcycle motorcycle)
  {
    return new MotorcyclesUnderAnalysis(
      motorcycle, 
      motorcycle.Model,
      motorcycle.LicensePlate,
      motorcycle.Year,
      DateTime.UtcNow);
  }
}

using MyMotorcycleService.Domain.Entities;

namespace MyMotorcycleService.Domain.Interfaces;

public interface IMotorcycleRepository
{
  Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate);
  Task<Motorcycle?> GetByIdAsync(Guid id);
  Task<IEnumerable<Motorcycle>?> GetAllAsync();
  Task AddAsync(Motorcycle motorcycle);
  void Update(Motorcycle rental);
  Task<Motorcycle?> RemoveByLicensePlate(string licensePlate);
  Task<Motorcycle?> RemoveById(Guid id);
}

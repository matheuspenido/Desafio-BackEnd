using MyMotorcycleService.Domain.Interfaces;

namespace MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMotorcycleRepository MotorcycleRepository { get; }
    int Complete();
    Task<int> CompleteAsync();
}

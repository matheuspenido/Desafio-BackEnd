using Microsoft.EntityFrameworkCore;

namespace MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;

public interface IAppDbContext : IDisposable
{
  DbSet<T> Set<T>() where T : class;
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  int SaveChanges();
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;

public interface IAppDbContext : IDisposable
{
  DbSet<T> Set<T>() where T : class;
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  int SaveChanges();
  Task<IDbContextTransaction> BeginTransactionAsync();
  Task CommitTransactionAsync();
  Task RollbackTransactionAsync();
}

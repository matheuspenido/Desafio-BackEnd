using Microsoft.EntityFrameworkCore.Storage;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork.Interfaces;

namespace MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly IAppDbContext _appDbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(IAppDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }

    public async Task BeginTransactionAsync()
    {
      if (_transaction == null)
        _transaction = await _appDbContext.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
      if (_transaction == null)
        throw new InvalidOperationException("No transaction has been started. Call BeginTransactionAsync first.");

      try
      {
        await _appDbContext.SaveChangesAsync();
        await _appDbContext.CommitTransactionAsync();
      }
      catch
      {
        await RollbackAsync();
        throw;
      }
      finally
      {
        _transaction = null;
      }
    }

    public async Task RollbackAsync()
    {
      if (_transaction == null)
        throw new InvalidOperationException("No transaction has been started. Call BeginTransactionAsync first.");

      await _appDbContext.RollbackTransactionAsync();
      _transaction = null;
    }

    public void Dispose()
    {
      _transaction?.Dispose();
      _appDbContext.Dispose();
    }
  }
}

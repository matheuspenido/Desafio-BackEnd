namespace MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork.Interfaces;

public interface IUnitOfWork : IDisposable
{
  Task BeginTransactionAsync();
  Task CommitAsync();
  Task RollbackAsync();
}

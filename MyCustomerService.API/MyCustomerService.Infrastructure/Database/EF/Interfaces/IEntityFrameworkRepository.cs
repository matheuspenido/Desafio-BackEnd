using MyCustomerService.Domain.Interfaces;

namespace MyCustomerService.Infrastructure.Database.EF.Interfaces;

public interface IEntityFrameworkRepository<TEntity> : IEntityBaseRepository<TEntity> where TEntity : class
{
  public IQueryable<TEntity> GetQueryable();
}

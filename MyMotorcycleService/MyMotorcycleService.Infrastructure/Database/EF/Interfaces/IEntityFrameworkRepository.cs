using MyMotorcycleService.Domain.Interfaces;

namespace MyMotorcycleService.Infrastructure.Database.EF.Interfaces;

public interface IEntityFrameworkRepository<TEntity> : Domain.Interfaces.IEntityBaseRepository<TEntity> where TEntity : class
{
    public IQueryable<TEntity> GetQueryable();
}

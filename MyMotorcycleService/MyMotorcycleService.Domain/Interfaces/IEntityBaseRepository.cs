namespace MyMotorcycleService.Domain.Interfaces;

public interface IEntityBaseRepository<TEntity> where TEntity : class
{
  Task<TEntity?> GetByIdAsync(Guid id);
  Task<IEnumerable<TEntity>?> GetAllAsync();
  Task AddAsync(TEntity entity);
  void Update(TEntity entity);
  void Remove(TEntity entity);
  Task SaveChangesAsync();
}
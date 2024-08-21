namespace MyRentMotorService.Domain.Interfaces;

public interface IEntityRepository<TEntity> where TEntity : class
{
  Task<TEntity?> GetByIdAsync(Guid id);
  Task<IEnumerable<TEntity>?> GetAllAsync();
  Task AddAsync(TEntity entity);
  void Update(TEntity entity);
  void Remove(TEntity entity);
  Task SaveChangesAsync();
}

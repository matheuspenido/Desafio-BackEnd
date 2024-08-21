using Microsoft.EntityFrameworkCore;
using MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyMotorcycleService.Infrastructure.Database.EF.Interfaces;

namespace MyMotorcycleService.Infrastructure.Database.EF;

public class EntityFrameworkRepository<TEntity> : IEntityFrameworkRepository<TEntity> where TEntity : class
{

  private readonly IAppDbContext _context;
  private readonly DbSet<TEntity> _dbSet;

  public EntityFrameworkRepository(IAppDbContext context)
  {
    _context = context;
    _dbSet = _context.Set<TEntity>();
  }

  public async Task AddAsync(TEntity entity)
  {
    await _dbSet.AddAsync(entity);
  }

  public async Task<IEnumerable<TEntity>?> GetAllAsync()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task<TEntity?> GetByIdAsync(Guid id)
  {
    return await _dbSet.FindAsync(id);
  }

  public void Remove(TEntity entity)
  {
    _dbSet.Remove(entity);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public void Update(TEntity entity)
  {
    _dbSet.Update(entity);
  }

  public IQueryable<TEntity> GetQueryable()
  {
    return _dbSet.AsQueryable();
  }
}

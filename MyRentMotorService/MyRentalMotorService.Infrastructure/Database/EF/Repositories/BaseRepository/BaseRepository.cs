using Microsoft.EntityFrameworkCore;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentMotorService.Domain.Interfaces;

namespace MyRentalMotorService.Infrastructure.Database.EF.Repositories.BaseRepository;

public abstract class BaseRepository<TEntity> : IEntityRepository<TEntity>
  where TEntity : class
{
    private readonly IAppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(IAppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }
    public IQueryable<TEntity> Queryable => _dbSet.AsQueryable();

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

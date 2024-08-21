using Microsoft.EntityFrameworkCore;
using MyMotorcycleService.Domain.Entities;
using MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;

namespace MyMotorcycleService.Infrastructure.Database.EF.Contexts;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

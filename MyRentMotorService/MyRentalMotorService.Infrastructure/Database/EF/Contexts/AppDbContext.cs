using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentalMotorService.Infrastructure.Database.EF.Contexts;

public class AppDbContext : DbContext, IAppDbContext
{
  public DbSet<Rental> Rentals { get; set; }
  public DbSet<Motorcycle> Motorcycles { get; set; }
  public DbSet<Customer> Customers { get; set; }

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

  public async Task<IDbContextTransaction> BeginTransactionAsync()
  {
    return await Database.BeginTransactionAsync();
  }

  public async Task CommitTransactionAsync()
  {
    if (Database.CurrentTransaction != null)
      await Database.CurrentTransaction.CommitAsync();
  }

  public async Task RollbackTransactionAsync()
  {
    if (Database.CurrentTransaction != null)
      await Database.CurrentTransaction.RollbackAsync();
  }
}

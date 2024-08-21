using Microsoft.EntityFrameworkCore;
using MyCustomerService.Domain.Entities;
using MyCustomerService.Infrastructure.Database.EF.Contexts.Interfaces;

namespace MyCustomerService.Infrastructure.Database.EF.Contexts;

public class AppDbContext : DbContext, IAppDbContext
{
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
}

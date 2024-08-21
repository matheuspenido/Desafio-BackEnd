using Microsoft.EntityFrameworkCore;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentalMotorService.Infrastructure.Database.EF.Repositories.BaseRepository;
using MyRentalMotorService.Infrastructure.Database.EF.Repositories.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;

namespace MyRentalMotorService.Infrastructure.Database.EF.Repositories;

public class RentalRepository : BaseRepository<Rental>, IRentalRepository
{
  public RentalRepository(IAppDbContext appDbContext) : base(appDbContext)
  {
  }

  public async override Task<Rental?> GetByIdAsync(Guid id)
  {
    var response = await Queryable
      .Include(r => r.Customer)
      .Include(r => r.Motorcycle)
      .FirstOrDefaultAsync(r => r.Id == id);

    return response;
  }

  public async override Task<IEnumerable<Rental>?> GetAllAsync()
  {
    var response = await Queryable
      .Include(r => r.Customer)
      .Include(r => r.Motorcycle)
      .ToListAsync();

    return response;
  }

  public async Task<Rental?> GetRentalByDriverLicense(string driverLicense)
  {
    var response = await Queryable
      .Include(r => r.Customer)
      .Include(r => r.Motorcycle)
      .FirstOrDefaultAsync(r => r.Customer.DriverLicense == driverLicense && r.ReturnDate == null);

    return response;
  }
}

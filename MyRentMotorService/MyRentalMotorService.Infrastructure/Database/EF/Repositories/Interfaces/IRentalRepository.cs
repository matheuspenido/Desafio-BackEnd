using MyRentMotorService.Domain.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;

namespace MyRentalMotorService.Infrastructure.Database.EF.Repositories.Interfaces;

public interface IRentalRepository : IEntityRepository<Rental>
{
  Task<Rental?> GetRentalByDriverLicense(string driverLicense);
}

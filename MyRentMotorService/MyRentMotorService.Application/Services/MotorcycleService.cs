using Microsoft.EntityFrameworkCore;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentMotorService.Application.Services.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentMotorService.Application.Services;

public class MotorcycleService : IMotorcycleService
{
  private readonly IAppDbContext _context;

  public MotorcycleService(IAppDbContext context)
  {
    _context = context;
  }

  public async Task<Motorcycle> GetMotorcycleByIdAsync(Guid id)
  {
    var result = await _context.Set<Motorcycle>().FindAsync(id);
    ArgumentNullException.ThrowIfNull(result);

    return result;
  }

  public async Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate)
  {
    var result = await _context.Set<Motorcycle>().SingleOrDefaultAsync(c => c.LicensePlate == licensePlate.ToUpper());
    ArgumentNullException.ThrowIfNull(result);

    return result;
  }
}

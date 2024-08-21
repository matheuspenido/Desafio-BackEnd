//using Microsoft.EntityFrameworkCore;
//using MyMotorcycleService.Domain.Entities;
//using MyMotorcycleService.Domain.Interfaces;
//using MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;

//namespace MyMotorcycleService.Infrastructure.Repositories;

//public class MotorcycleRepository : IMotorcycleRepository
//{
//  private readonly IAppDbContext _context;

//  public MotorcycleRepository(IAppDbContext context)
//  {
//    _context = context;
//  }

//  public async Task AddAsync(Motorcycle motorcycle)
//  {
//    await _context.Motorcycles.AddAsync(motorcycle);
//  }

//  public async Task <IEnumerable<Motorcycle>?> GetAllAsync()
//  {
//    return await _context.Motorcycles.ToListAsync();
//  }

//  public async Task<Motorcycle?> GetByIdAsync(Guid id)
//  {
//    return await _context.Motorcycles.FindAsync(id);
//  }

//  public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate)
//  {
//    return await _context.Motorcycles
//      .SingleOrDefaultAsync(r => r.LicensePlate == licensePlate);
//  }

//  public async Task<Motorcycle?> RemoveById(Guid id)
//  {
//    var motorcycle = await _context.Motorcycles.FindAsync(id);
//    if (motorcycle != null)
//      _context.Motorcycles.Remove(motorcycle);

//    return motorcycle;
//  }

//  public async Task<Motorcycle?> RemoveByLicensePlate(string licensePlate)
//  {
//    var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
//    if (motorcycle != null)
//      _context.Motorcycles.Remove(motorcycle);

//    return motorcycle;
//  }

//  public void Update(Motorcycle motorcycle)
//  {
//    _context.Motorcycles.Update(motorcycle);
//  }
//}

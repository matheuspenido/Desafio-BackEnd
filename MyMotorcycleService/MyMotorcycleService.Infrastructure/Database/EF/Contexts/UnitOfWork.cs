//using MyMotorcycleService.Domain.Interfaces;
//using MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;
//using MyMotorcycleService.Infrastructure.Repositories;

//namespace MyMotorcycleService.Infrastructure.Database.EF.Contexts;

//public class UnitOfWork : IUnitOfWork
//{
//    private readonly IAppDbContext _context;
//    private MotorcycleRepository? _motorcycleRepository;

//    public UnitOfWork(IAppDbContext context)
//    {
//        _context = context;
//    }

//    //Just add new repositories here, to allow the application became more reliable in the future.
//    public IMotorcycleRepository MotorcycleRepository => _motorcycleRepository ??= new MotorcycleRepository(_context);

//    public int Complete()
//    {
//        return _context.SaveChanges();
//    }

//    public async Task<int> CompleteAsync()
//    {
//        return await _context.SaveChangesAsync();
//    }

//    public void Dispose()
//    {
//        _context.Dispose();
//    }

//}

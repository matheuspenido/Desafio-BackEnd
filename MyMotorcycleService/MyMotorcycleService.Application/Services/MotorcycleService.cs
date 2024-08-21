using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;
using MyMotorcycleService.Application.Dtos.Requests;
using MyMotorcycleService.Application.Dtos.Responses;
using MyMotorcycleService.Application.Services.Interfaces;
using MyMotorcycleService.Domain.Entities;
using MyMotorcycleService.Infrastructure.Database.EF.Interfaces;
using System.Text.RegularExpressions;

namespace MyMotorcycleService.Application.Services;

public class MotorcycleService : IMotorcycleService
{
  private readonly IEntityFrameworkRepository<Motorcycle> _motorcycleRepository;
  private readonly IEventPublisher<CrudEntityEvent<MotorcycleBusEntity>> _eventPublisher;
  private readonly IMapper _mapper;
  private readonly ILogger _logger;

  public MotorcycleService(IEntityFrameworkRepository<Motorcycle> motorcycleRepository, IEventPublisher<CrudEntityEvent<MotorcycleBusEntity>> eventPublisher, IMapper mapper, ILogger<MotorcycleService> logger)
  {
    _motorcycleRepository = motorcycleRepository;
    _eventPublisher = eventPublisher;
    _mapper = mapper;
    _logger = logger;
  }

  public async Task<MotorcycleResponseDto?> GetMotorcycleByLicensePlate(string licensePlate)
  {
    var motorcycle = await FindMotorCycleByLicensePlateAsync(licensePlate);

    if (motorcycle == null)
      throw new Exception($"Motorcycle for {licensePlate} not found.");

    return _mapper.Map<MotorcycleResponseDto>(motorcycle);
  }

  public async Task AddMotorcycle(AddMotorcycleRequestDto motorcycleDto)
  {
    var existingMotorcycle = await FindMotorCycleByLicensePlateAsync(motorcycleDto.LicensePlate);

    if (existingMotorcycle is not null)
      throw new Exception($"Motorcycle for {existingMotorcycle.LicensePlate} already exists.");

    var newMotorcycle = await AddMotorcycleAsync(motorcycleDto);

    var eventMessage = CreateEvent(newMotorcycle, CrudEnum.Created);
    await _eventPublisher.PublishAsync(eventMessage);
  }

  public async Task UpdateMotorcycle(UpdateMotorcycleRequestDto motorcycleDto)
  {
    var existingMotorcycle = await FindMotorCycleByLicensePlateAsync(motorcycleDto.LicensePlate);

    if (existingMotorcycle == null)
      throw new Exception($"Motorcycle for {motorcycleDto.LicensePlate} not found.");

    var updatedMotorcycle = await UpdateMotorcycleAsync(existingMotorcycle, motorcycleDto);

    _logger.LogInformation("Updated Motorcyle, for Motorcycle License Plate: {LicensePlate}", motorcycleDto?.LicensePlate);
    await _motorcycleRepository.SaveChangesAsync();

    var eventMessage = CreateEvent(updatedMotorcycle, CrudEnum.Updated);
    await _eventPublisher.PublishAsync(eventMessage);
  }

  public async Task RemoveMotorcycleByLicensePlate(string licensePlate)
  {
    var motorcycle = await FindMotorCycleByLicensePlateAsync(licensePlate);

    if (motorcycle == null)
      throw new Exception($"Motorcycle for {licensePlate} not found.");

    await RemoveMotorcycleAsync(motorcycle);

    if (motorcycle != null)
    {
      var eventMessage = CreateEvent(motorcycle, CrudEnum.Deleted);
      await _eventPublisher.PublishAsync(eventMessage);
    }
  }

  public async Task<MotorcycleResponseDto?> UpdateLicensePlate(string licensePlate, PatchLicensePlateDto patchLicencePlateDto)
  {
    var existingMotorcycle = await FindMotorCycleByLicensePlateAsync(licensePlate);
    Motorcycle? updatedMotorcycle = null;

    if (existingMotorcycle is null)
      throw new Exception($"Customer for {patchLicencePlateDto.LicensePlate} license plate doesn't exists.");
   
    updatedMotorcycle = await ReplaceMotorcycleLicensePlateAsync(existingMotorcycle, patchLicencePlateDto.LicensePlate);

    var eventMessage = CreateEvent(updatedMotorcycle, CrudEnum.Updated);
    await _eventPublisher.PublishAsync(eventMessage);
    
    return _mapper.Map<MotorcycleResponseDto>(updatedMotorcycle);
  }

  public async Task<IEnumerable<MotorcycleResponseDto>?> GetAllMotorcycles()
  {
    var response = await _motorcycleRepository.GetAllAsync();

    return _mapper.Map<IEnumerable<MotorcycleResponseDto>>(response);
  }

  private CrudEntityEvent<MotorcycleBusEntity> CreateEvent(Motorcycle motorcycle, CrudEnum eventType)
  {
    var customerBusEntity = new MotorcycleBusEntity
    {
      Id = motorcycle.Id,
      Model = motorcycle.Model,
      LicensePlate = motorcycle.LicensePlate,
      Year = motorcycle.Year
    };

    var motorcycleEvent = new CrudEntityEvent<MotorcycleBusEntity>(eventType, customerBusEntity);
    return motorcycleEvent;
  }

  private string NormalizeLicensePlate(string licensePlate)
  {
    return Regex.Replace(licensePlate, @"\s+", "").ToUpper();
  }

  private async Task<Motorcycle?> FindMotorCycleByLicensePlateAsync(string licensePlate)
  {
    var normalizedLicensePlate = NormalizeLicensePlate(licensePlate);

    var motorcycle = await _motorcycleRepository.GetQueryable()
      .SingleOrDefaultAsync(r => r.LicensePlate == normalizedLicensePlate);

    return motorcycle;
  }

  private async Task<Motorcycle> AddMotorcycleAsync(AddMotorcycleRequestDto motorcycleDto)
  {
    var normalizedLicensePlate = NormalizeLicensePlate(motorcycleDto.LicensePlate);

    var newMotorcycleEntity = new Motorcycle(Guid.NewGuid(), motorcycleDto.Model, normalizedLicensePlate, motorcycleDto.Year, true);
    await _motorcycleRepository.AddAsync(newMotorcycleEntity);

    await _motorcycleRepository.SaveChangesAsync();

    return newMotorcycleEntity;
  }

  private async Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle motorcycleToBeUpdated, UpdateMotorcycleRequestDto motorcycleDto)
  {
    var normalizedLicensePlate = NormalizeLicensePlate(motorcycleDto.LicensePlate);

    motorcycleToBeUpdated.MotorcycleUpdate(motorcycleDto.Model, normalizedLicensePlate, motorcycleDto.Year, motorcycleDto.IsAvailable);

    _motorcycleRepository.Update(motorcycleToBeUpdated);
    await _motorcycleRepository.SaveChangesAsync();

    return motorcycleToBeUpdated;
  }

  private async Task RemoveMotorcycleAsync(Motorcycle motorcycle)
  {
    if (!motorcycle.IsAvailable)
      throw new Exception("Motorcycle in use. It cannot be removed.");
    _motorcycleRepository.Remove(motorcycle);
    await _motorcycleRepository.SaveChangesAsync();
  }

  private async Task<Motorcycle> ReplaceMotorcycleLicensePlateAsync(Motorcycle motorcycle, string licensePlate)
  {
    await RemoveMotorcycleAsync(motorcycle);

    var normalizedLicensePlate = NormalizeLicensePlate(licensePlate);
    motorcycle.UpdateLicensePlate(normalizedLicensePlate);

    await _motorcycleRepository.AddAsync(motorcycle);
    await _motorcycleRepository.SaveChangesAsync();

    return motorcycle;
  }
}

using AutoMapper;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyRentalMotorService.Infrastructure.Database.EF.Repositories.Interfaces;
using MyRentMotorService.Application.Dto;
using MyRentMotorService.Application.Dtos.Requests;
using MyRentMotorService.Application.Dtos.Responses;
using MyRentMotorService.Application.Services.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;
using MyRentMotorService.Domain.RentalAggregate.Entities;
using MyRentMotorService.Domain.RentalAggregate.Enums;

namespace MyRentMotorService.Application.Services;

public class RentalService : IRentalService
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICustomerService _customerService;
  private readonly IMotorcycleService _motorcycleService;
  private readonly IEventPublisher<CrudEntityEvent<MotorcycleStatusBusEntity>> _motorcycleStatusEventPublisher;
  private readonly IEventPublisher<CrudEntityEvent<CustomerStatusBusEntity>> _customerStatusEventPublisher;
  private readonly IMapper _mapper;

  public RentalService(
    IRentalRepository rentalRepository, 
    ICustomerService customerService,
    IMotorcycleService motorcycleService, 
    IEventPublisher<CrudEntityEvent<MotorcycleStatusBusEntity>> motorcycleStatusEventPublisher,
    IEventPublisher<CrudEntityEvent<CustomerStatusBusEntity>> customereStatusEventPublisher,
    IMapper mapper)
  {
    _rentalRepository = rentalRepository;
    _customerService = customerService;
    _motorcycleService = motorcycleService;
    _motorcycleStatusEventPublisher = motorcycleStatusEventPublisher;
    _customerStatusEventPublisher = customereStatusEventPublisher;
    _mapper = mapper;
  }

  public async Task<ResponseCloseRentalApplicationDto> CompleteRentalAsync(RequestCompleteRentalApplicationDto completeRentalDto)
  {
    var rental = await _rentalRepository.GetByIdAsync(completeRentalDto.Id);

    if (rental == null)
      throw new Exception("Rental not found.");

    rental.ReturnMotorcycle(completeRentalDto.ReturnDate);
    await _rentalRepository.SaveChangesAsync();

    var motorcycleStatusEvent = CreateMotorcycleEvent(rental, CrudEnum.Updated);
    var customerStatusEvent = CreateCustomerEvent(rental, CrudEnum.Updated);

    await _motorcycleStatusEventPublisher.PublishAsync(motorcycleStatusEvent);
    await _customerStatusEventPublisher.PublishAsync(customerStatusEvent);

    return _mapper.Map<ResponseCloseRentalApplicationDto>(rental);
  }

  public async Task<ResponseCreateRentalApplicationDto> CreateRentalAsync(CreateRentalApplicationDto createRentalDto)
  {
    var customer = await _customerService.GetCustomerByLicenseDriverAsync(createRentalDto.DriverLicense);
    var motorcycle = await _motorcycleService.GetMotorcycleByLicensePlateAsync(createRentalDto.LicensePlate);

    if (!motorcycle.IsAvailable)
      throw new Exception("Motorcycle already rented.");

    var estimatedReturnDate = createRentalDto.StartDate.AddDays((int)createRentalDto.RentalPlan);
    var startDate = createRentalDto.StartDate;

    var rentalPlan = _mapper.Map<RentalPlanEnum>(createRentalDto.RentalPlan);
    var rental = new Rental(motorcycle, customer, startDate, estimatedReturnDate, rentalPlan);

    await _rentalRepository.AddAsync(rental);
    await _rentalRepository.SaveChangesAsync();

    var motorcycleStatusEvent = CreateMotorcycleEvent(rental, CrudEnum.Updated);
    var customerStatusEvent = CreateCustomerEvent(rental, CrudEnum.Updated);

    await _motorcycleStatusEventPublisher.PublishAsync(motorcycleStatusEvent);
    await _customerStatusEventPublisher.PublishAsync(customerStatusEvent);

    return _mapper.Map<ResponseCreateRentalApplicationDto>(rental);
  }

  public async Task<ResponseGetRentalApplicationDto?> GetRentalByIdAsync(Guid rentalId)
  {
    var rental = await _rentalRepository.GetByIdAsync(rentalId);

    if (rental == null)
      throw new Exception("Rental not found.");

    return _mapper.Map<ResponseGetRentalApplicationDto>(rental);
  }

  public async Task<ResponseGetCostPreviewApplicationDto?> GetRentalPaymentPreviewAsync(RequestPreviewInfoApplicationDto requestPreviewInfo)
  {
    var rental = await _rentalRepository.GetRentalByDriverLicense(requestPreviewInfo.DriverLicense);

    if (rental == null)
      throw new Exception("Rental not found or closed.");

    var preview = _mapper.Map<ResponseGetCostPreviewApplicationDto>(rental);
    
    preview.EstimatedReturnDate = requestPreviewInfo.ReturnDate;
    preview.EstimatedPrice = rental.CalculateCost(requestPreviewInfo.ReturnDate);

    return preview;
  }

  private CrudEntityEvent<CustomerStatusBusEntity> CreateCustomerEvent(Rental rental, CrudEnum eventType)
  {
    var customer = rental.Customer;

    var customerBusEntity = new CustomerStatusBusEntity
    {
      Id = customer.Id,
      IsActive = customer.IsActive
    };

    var customerEvent = new CrudEntityEvent<CustomerStatusBusEntity>(eventType, customerBusEntity);
    return customerEvent;
  }

  private CrudEntityEvent<MotorcycleStatusBusEntity> CreateMotorcycleEvent(Rental rental, CrudEnum eventType)
  {
    var motorcycle = rental.Motorcycle;

    var motorcycleBusEntity = new MotorcycleStatusBusEntity
    {
      Id = motorcycle.Id,
      IsAvailable = motorcycle.IsAvailable
    };

    var motorcycleEvent = new CrudEntityEvent<MotorcycleStatusBusEntity>(eventType, motorcycleBusEntity);
    return motorcycleEvent;
  }

  public async Task<IEnumerable<ResponseGetRentalApplicationDto?>> GetAllRentalsAsync()
  {
    var rentals = await _rentalRepository.GetAllAsync();

    return _mapper.Map<IEnumerable<ResponseGetRentalApplicationDto>>(rentals);
  }
}

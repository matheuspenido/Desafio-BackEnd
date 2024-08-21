using AutoMapper;
using MyCustomerService.Application.Dtos.Requests;
using MyCustomerService.Application.Dtos.Responses;
using MyCustomerService.Application.Services.Interfaces;
using MyCustomerService.Domain.Entities;
using MyCustomerService.Domain.Interfaces;
using MyCustomerService.Infrastructure.Messaging.Extensions;
using MyCustomerService.Infrastructure.Database.EF.Interfaces;
using MyMessageContracts.Contracts;
using MyMessageContracts.SyncEntities.Events;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;

namespace MyCustomerService.Application.Services;

public class CustomerService : IEntityRepository
{
  private readonly IEntityFrameworkRepository<Customer> _customerRepository;
  private readonly IEventPublisher<CrudEntityEvent<CustomerBusEntity>> _eventPublisher;
  private readonly IFileRepository _fileRepository;
  private readonly IMapper _mapper;

  public CustomerService(IEntityFrameworkRepository<Customer> customerRepository, IEventPublisher<CrudEntityEvent<CustomerBusEntity>> customerEventPublish, IFileRepository fileRepository, IMapper mapper)
  {
    _eventPublisher = customerEventPublish;
    _customerRepository = customerRepository;
    _fileRepository = fileRepository;
    _mapper = mapper;
  }

  public async Task<CustomerResponseDto?> GetCustomerByDriverLicense(string driverLicense)
  {
    var customer = await FindCustomerByDriverLicenseAsync(driverLicense);

    if (customer == null)
      throw new Exception($"Customer for {driverLicense} driver license not found.");

    return _mapper.Map<CustomerResponseDto>(customer);
  }

  public async Task<CustomerResponseDto?> GetCustomerByCnpj(string cnpj)
  {
    var customer = await FindCustomerByCnpjAsync(cnpj);

    if (customer == null)
      throw new Exception($"Customer for {cnpj} cnpj not found.");

    return _mapper.Map<CustomerResponseDto>(customer);
  }

  public async Task AddCustomer(AddCustomerRequestDto customerDto)
  {
    var existingCustomer = await FindCustomerByDriverLicenseAsync(customerDto.DriverLicense);

    var normalizedDriverLicense = NormalizedIdentifiers(customerDto.DriverLicense);
    var normalizedCnpj = NormalizedIdentifiers(customerDto.Cnpj);

    if (existingCustomer is not null)
      throw new Exception($"Customer for {existingCustomer.DriverLicense} already exists.");

    var newCustomer = await AddCustomerAsync(customerDto);

    var eventMessage = CreateEvent(newCustomer, CrudEnum.Created);
    await _eventPublisher.PublishAsync(eventMessage);
  }

  public async Task UpdateCustomer(UpdateCustomerRequestDto customerDto)
  {
    var existingCustomer = await FindCustomerByDriverLicenseAsync(customerDto.DriverLicense);

    var normalizedDriverLicense = NormalizedIdentifiers(customerDto.DriverLicense);
    var normalizedCnpj = NormalizedIdentifiers(customerDto.Cnpj);

    if (existingCustomer is null)
      throw new Exception($"Customer for {customerDto.DriverLicense} driver license doesn't exists.");

    var updatedMotorcycle = await UpdateCustomerAsync(existingCustomer, customerDto);

    var eventMessage = CreateEvent(existingCustomer, CrudEnum.Updated);
    await _eventPublisher.PublishAsync(eventMessage);
  }

  public async Task RemoveCustomerByCnpj(string cnpj)
  {
    var customer = await FindCustomerByCnpjAsync(cnpj);

    if (customer == null)
      throw new Exception($"Customer for cnpj {cnpj} not found.");

    if (customer.ActiveCustomer)
      throw new Exception("Active Customer. It cannot be removed.");

    await RemoveCustomerAsync(customer);

    if (customer != null)
    {
      var eventMessage = CreateEvent(customer, CrudEnum.Updated);
      await _eventPublisher.PublishAsync(eventMessage);
    }
  }

  public async Task RemoveCustomerByDriverLicense(string driverLicense)
  {
    var customer = await FindCustomerByDriverLicenseAsync(driverLicense);

    if (customer == null)
      throw new Exception($"Customer for driver license {driverLicense} not found.");

    if (customer.ActiveCustomer)
      throw new Exception("Active Customer. It cannot be removed.");

    await RemoveCustomerAsync(customer);

    if (customer != null)
    {
      var eventMessage = CreateEvent(customer, CrudEnum.Updated);
      await _eventPublisher.PublishAsync(eventMessage);
    }
  }

  public async Task<IEnumerable<CustomerResponseDto>?> GetAllCustomers()
  {
    var response = await _customerRepository.GetAllAsync();

    return _mapper.Map<IEnumerable<CustomerResponseDto>>(response);
  }

  private CrudEntityEvent<CustomerBusEntity> CreateEvent(Customer customer, CrudEnum eventType)
  {
    var customerBusEntity = new CustomerBusEntity
    {
      Id = customer.Id,
      Name = customer.Name,
      BirthDate = customer.BirthDate,
      Cnpj = customer.Cnpj,
      DriverLicense = customer.DriverLicense,
      DriverLicenseImageLocation = customer.DriverLicenseImageLocation,
      DriverLicenseType = customer.DriverLicenseType.ToEventEnum()
    };

    var customerEvent = new CrudEntityEvent<CustomerBusEntity>(eventType, customerBusEntity);
    return customerEvent;
  }

  private async Task<Customer?> FindCustomerByDriverLicenseAsync(string driverLicense)
  {
    var normalizedDriverLicense = NormalizedIdentifiers(driverLicense);

    var customer = await _customerRepository.GetQueryable()
      .SingleOrDefaultAsync(r => r.DriverLicense == normalizedDriverLicense);

    return customer;
  }

  private async Task<Customer?> FindCustomerByCnpjAsync(string cnpj)
  {
    var normalizedCnpj = NormalizedIdentifiers(cnpj);

    var customer = await _customerRepository.GetQueryable()
      .SingleOrDefaultAsync(r => r.Cnpj == normalizedCnpj);

    return customer;
  }

  private async Task<Customer> AddCustomerAsync(AddCustomerRequestDto customerDto)
  {
    var normalizedDriverLicense = NormalizedIdentifiers(customerDto.DriverLicense);
    var normalizedCnpj = NormalizedIdentifiers(customerDto.Cnpj);

    var newCustomer = new Customer(Guid.NewGuid(), customerDto.Name, normalizedCnpj, customerDto.BirthDate, normalizedDriverLicense, customerDto.DriverLicenseType, false);

    await _customerRepository.AddAsync(newCustomer);
    await _customerRepository.SaveChangesAsync();

    return newCustomer;
  }

  private async Task<Customer> UpdateCustomerAsync(Customer customerToBeUpdated, UpdateCustomerRequestDto customerDto)
  {
    var normalizedDriverLicense = NormalizedIdentifiers(customerDto.DriverLicense);
    var normalizedCnpj = NormalizedIdentifiers(customerDto.Cnpj);

    customerToBeUpdated.CustomerUpdate(
        customerDto.Name,
        normalizedCnpj,
        customerDto.BirthDate,
        normalizedDriverLicense,
        customerDto.DriverLicenseType);

    _customerRepository.Update(customerToBeUpdated);
    await _customerRepository.SaveChangesAsync();

    return customerToBeUpdated;
  }

  private async Task RemoveCustomerAsync(Customer customer)
  {
    _customerRepository.Remove(customer);
    await _customerRepository.SaveChangesAsync();
  }

  private string NormalizedIdentifiers(string driverLicense)
  {
    return Regex.Replace(driverLicense, @"\s+", "").ToUpper();
  }
}
using Microsoft.EntityFrameworkCore;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentMotorService.Application.Services.Interfaces;

public interface IMotorcycleService
{
  Task<Motorcycle> GetMotorcycleByIdAsync(Guid id);
  Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate);
}

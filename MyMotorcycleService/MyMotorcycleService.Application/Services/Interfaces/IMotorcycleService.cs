using MyMotorcycleService.Application.Dtos.Requests;
using MyMotorcycleService.Application.Dtos.Responses;

namespace MyMotorcycleService.Application.Services.Interfaces;

public interface IMotorcycleService
{
  Task<MotorcycleResponseDto?> GetMotorcycleByLicensePlate(string licencePlate);
  Task AddMotorcycle(AddMotorcycleRequestDto motorcycle);
  Task UpdateMotorcycle(UpdateMotorcycleRequestDto motorcycle);
  Task RemoveMotorcycleByLicensePlate(string licensePlate);
  Task<MotorcycleResponseDto?> UpdateLicensePlate(string licensePlate, PatchLicensePlateDto patchLicensePlateDto);
  Task<IEnumerable<MotorcycleResponseDto>?> GetAllMotorcycles();
}

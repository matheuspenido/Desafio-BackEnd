using MyRentMotorService.Application.Dto;
using MyRentMotorService.Application.Dtos.Requests;
using MyRentMotorService.Application.Dtos.Responses;

namespace MyRentMotorService.Application.Services.Interfaces;

public interface IRentalService
{
  Task<ResponseCreateRentalApplicationDto> CreateRentalAsync(CreateRentalApplicationDto createRentalDto);
  Task<ResponseCloseRentalApplicationDto> CompleteRentalAsync(RequestCompleteRentalApplicationDto requestPreviewInfo);
  Task<ResponseGetRentalApplicationDto?> GetRentalByIdAsync(Guid rentalId);
  Task<ResponseGetCostPreviewApplicationDto?> GetRentalPaymentPreviewAsync(RequestPreviewInfoApplicationDto requestPreviewInfo);
  Task<IEnumerable<ResponseGetRentalApplicationDto?>> GetAllRentalsAsync();
}

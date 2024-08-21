using AutoMapper;
using MyRentMotorService.API.Dtos;
using MyRentMotorService.API.Dtos.Enums;
using MyRentMotorService.Application.Dtos.Enums;
using MyRentMotorService.Application.Dtos.Requests;

namespace MyRentalMotorService.Application.Mappings;

public class MappingApiDtosProfile : Profile
{
  public MappingApiDtosProfile()
  {
    CreateMap<RentalPlanEnumApplicationDto, RequestRentalPlanEnum>()
      .ReverseMap();

    CreateMap<CreateRentalApplicationDto, RequestCreateRentalDto>()
      .ReverseMap();
  }
}

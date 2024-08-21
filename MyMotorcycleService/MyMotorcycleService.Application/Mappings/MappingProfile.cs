using AutoMapper;
using MyMotorcycleService.Application.Dtos.Requests;
using MyMotorcycleService.Application.Dtos.Responses;
using MyMotorcycleService.Domain.Entities;

namespace MyMotorcycleService.Application.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<AddMotorcycleRequestDto, Motorcycle>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.IsAvailable, opt => opt.Ignore());

    CreateMap<UpdateMotorcycleRequestDto, Motorcycle>();

    CreateMap<Motorcycle, MotorcycleResponseDto>()
      .ForSourceMember(dest => dest.Id, opt => opt.DoNotValidate());
  }
}

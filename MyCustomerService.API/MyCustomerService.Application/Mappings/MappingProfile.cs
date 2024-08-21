using AutoMapper;
using MyCustomerService.Application.Dtos.Requests;
using MyCustomerService.Application.Dtos.Responses;
using MyCustomerService.Domain.Entities;

namespace MyCustomerService.Application.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<AddCustomerRequestDto, Customer>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.DriverLicenseImageLocation, opt => opt.Ignore());

    CreateMap<UpdateCustomerRequestDto, Customer>();

    CreateMap<Customer, CustomerResponseDto>()
      .ForSourceMember(dest => dest.Id, opt => opt.DoNotValidate());
  }
}

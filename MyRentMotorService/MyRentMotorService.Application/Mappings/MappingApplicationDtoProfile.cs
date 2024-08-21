using AutoMapper;
using MyRentMotorService.Application.Dtos.Enums;
using MyRentMotorService.Application.Dtos.Responses;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;
using MyRentMotorService.Domain.RentalAggregate.Enums;

namespace MyRentalMotorService.Application.Mappings;

public class MappingApplicationDtoProfile : Profile
{
  public MappingApplicationDtoProfile()
  {
    CreateMap<RentalPlanEnumApplicationDto, RentalPlanEnum>()
      .ReverseMap();

    CreateMap<Rental, ResponseCreateRentalApplicationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.Motorcycle.Id))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Motorcycle.LicensePlate))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.DriverLicense, opt => opt.MapFrom(src => src.Customer.DriverLicense))
            .ForMember(dest => dest.RentalDate, opt => opt.MapFrom(src => src.RentalDate))
            .ForMember(dest => dest.EstimatedReturnDate, opt => opt.MapFrom(src => src.EstimatedReturnDate))
            .ForMember(dest => dest.EstimatedPrice, opt => opt.MapFrom(src => src.EstimatedPrice));

    CreateMap<Rental, ResponseCloseRentalApplicationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.Motorcycle.Id))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Motorcycle.LicensePlate))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.DriverLicense, opt => opt.MapFrom(src => src.Customer.DriverLicense))
            .ForMember(dest => dest.RentalDate, opt => opt.MapFrom(src => src.RentalDate))
            .ForMember(dest => dest.EstimatedReturnDate, opt => opt.MapFrom(src => src.EstimatedReturnDate))
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            .ForMember(dest => dest.EstimatedReturnDate, opt => opt.MapFrom(src => src.EstimatedReturnDate))
            .ForMember(dest => dest.PaidPrice, opt => opt.MapFrom(src => src.PaidPrice));

    CreateMap<Rental, ResponseGetRentalApplicationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.Motorcycle.Id))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Motorcycle.LicensePlate))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.DriverLicense, opt => opt.MapFrom(src => src.Customer.DriverLicense))
            .ForMember(dest => dest.RentalDate, opt => opt.MapFrom(src => src.RentalDate))
            .ForMember(dest => dest.EstimatedReturnDate, opt => opt.MapFrom(src => src.EstimatedReturnDate))
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            .ForMember(dest => dest.EstimatedPrice, opt => opt.MapFrom(src => src.EstimatedPrice))
            .ForMember(dest => dest.PaidPrice, opt => opt.MapFrom(src => src.PaidPrice));

    CreateMap<Rental, ResponseGetCostPreviewApplicationDto>()
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Motorcycle.Model))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Motorcycle.LicensePlate))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.DriverLicense, opt => opt.MapFrom(src => src.Customer.DriverLicense))
            .ForMember(dest => dest.RentalDate, opt => opt.MapFrom(src => src.RentalDate))
            .ForMember(dest => dest.EstimatedPrice, opt => opt.Ignore())
            .ForMember(dest => dest.EstimatedReturnDate, opt => opt.Ignore());
  }
}

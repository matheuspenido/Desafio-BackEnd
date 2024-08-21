using MyRentMotorService.Domain.RentalAggregate.Enums;

namespace MyRentalMotorService.Infrastructure.Messaging.Extensions;

public static class EnumConversionExtensions
{
  public static MyMessageContracts.Contracts.DriverLicenseTypeEnum ToEventEnum(this DriverLicenseTypeEnum entityEnum)
  {
    return (MyMessageContracts.Contracts.DriverLicenseTypeEnum)entityEnum;
  }

  public static DriverLicenseTypeEnum ToEntityEnum(this MyMessageContracts.Contracts.DriverLicenseTypeEnum entityEnum)
  {
    return (DriverLicenseTypeEnum)entityEnum;
  }
}

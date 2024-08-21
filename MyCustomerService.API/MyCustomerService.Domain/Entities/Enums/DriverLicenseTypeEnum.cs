using System.Runtime.Serialization;

namespace MyCustomerService.Domain.Entities.Enums;

public enum DriverLicenseTypeEnum
{
  [EnumMember(Value = "A")]
  A,
  [EnumMember(Value = "B")]
  B,
  [EnumMember(Value = "A+B")]
  AB
}

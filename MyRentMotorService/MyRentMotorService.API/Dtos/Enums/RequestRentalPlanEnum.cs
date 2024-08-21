using System.Runtime.Serialization;

namespace MyRentMotorService.API.Dtos.Enums;

public enum RequestRentalPlanEnum
{
  [EnumMember(Value = "7_Days")]
  SevenDaysPlan = 7,
  [EnumMember(Value = "15_Days")]
  FifteenDaysPlan = 15,
  [EnumMember(Value = "30_Days")]
  ThirtyDaysPlan = 30,
  [EnumMember(Value = "45_Days")]
  FortyFiveDaysPlan = 45,
  [EnumMember(Value = "50_Days")]
  FiftyDaysPlan = 50
}

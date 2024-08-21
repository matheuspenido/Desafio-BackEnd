using MyRentalMotorService.Infrastructure.Data.Aggregates.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;
using MyRentMotorService.Domain.RentalAggregate.Enums;

namespace MyRentMotorService.Domain.RentalAggregate.Aggregate;

public class Rental : IAggregateRoot
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public Motorcycle Motorcycle { get; private set; } = default!;
  public Customer Customer { get; private set; } = default!;
  public DateTime RentalDate { get; private set; } = default!;
  public DateTime EstimatedReturnDate { get; private set; } = default!;
  public RentalPlanEnum RentalPlan { get; private set; } = default!;
  public decimal? PaidPrice { get; set; }
  public decimal EstimatedPrice { get; set; }
  public DateTime? ReturnDate { get; private set; } = null;

  //For EF
  public Rental() { }

  public Rental(Motorcycle motorcycle, Customer customer, DateTime rentalDate, DateTime estimatedReturnDate, RentalPlanEnum rentalPlan)
  {
    if (customer.DriverLicenseType == DriverLicenseTypeEnum.B)
      throw new Exception($"Driver License category not allowed ({customer.DriverLicenseType})");

    Id = Guid.NewGuid();
    Motorcycle = motorcycle;
    Customer = customer;
    RentalDate = rentalDate.AddDays(1);
    EstimatedReturnDate = estimatedReturnDate.AddDays(1);
    RentalPlan = rentalPlan;

    Customer.MarkAsActiveCustomer();
    Motorcycle.MarkAsRented();

    EstimatedPrice = CalculateCost(EstimatedReturnDate);
  }

  public void ReturnMotorcycle(DateTime returnedDate)
  {
    ReturnDate = returnedDate;
    Motorcycle.MarkAsAvailable();
    Customer.MarkAsInactive();

    PaidPrice = CalculateCost(ReturnDate.Value);
  }

  public decimal CalculateCost(DateTime returnedDate)
  {
    decimal cost;

    int rentalDurationDays = (returnedDate.Date - RentalDate.Date).Days;

    if (rentalDurationDays < 0)
    {
      returnedDate = RentalDate;
      rentalDurationDays = 0;
    }

    decimal dailyRate = GetDailyRate(RentalPlan);
    cost = rentalDurationDays * dailyRate;

    if (rentalDurationDays < (int)RentalPlan)
    {
      decimal penaltyRate = GetPenaltyRate(RentalPlan);
      int unusedDays = (int)RentalPlan - rentalDurationDays;
      cost += unusedDays * dailyRate * penaltyRate;
    }
    else if (rentalDurationDays > (int)RentalPlan)
    {
      int exceededDays = rentalDurationDays - (int)RentalPlan;
      cost += exceededDays * 50m;
    }

    return cost;
  }

  private decimal GetDailyRate(RentalPlanEnum plan)
  {
    return plan switch
    {
      RentalPlanEnum.SevenDaysPlan => 30m,
      RentalPlanEnum.FifteenDaysPlan => 28m,
      RentalPlanEnum.ThirtyDaysPlan => 22m,
      RentalPlanEnum.FortyFiveDaysPlan => 20m,
      RentalPlanEnum.FiftyDaysPlan => 18m,
      _ => throw new ArgumentOutOfRangeException(nameof(plan), $"No daily rate defined for the rental plan {plan}.")
    };
  }

  private decimal GetPenaltyRate(RentalPlanEnum plan)
  {
    return plan switch
    {
      RentalPlanEnum.SevenDaysPlan => 0.2m,
      RentalPlanEnum.FifteenDaysPlan => 0.4m,
      RentalPlanEnum.ThirtyDaysPlan => 0.0m,
      RentalPlanEnum.FortyFiveDaysPlan => 0.0m,
      RentalPlanEnum.FiftyDaysPlan => 0.0m,
      _ => throw new ArgumentOutOfRangeException(nameof(plan), $"No penalty rate defined for the rental plan {plan}.")
    };
  }
}

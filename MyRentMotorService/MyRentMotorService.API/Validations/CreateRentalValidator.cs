using FluentValidation;
using MyRentMotorService.Application.Dtos.Requests;

namespace MyRentMotorService.API.Validations;

public class CreateRentalValidator : AbstractValidator<CreateRentalApplicationDto>
{
  public CreateRentalValidator()
  {
    RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.");

    RuleFor(x => x.DriverLicense)
              .NotEmpty().WithMessage("Driver license is required.");

    RuleFor(x => x.StartDate)
              .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start date must be in the future.");

    RuleFor(x => x.RentalPlan)
              .IsInEnum().WithMessage("Invalid rental plan.");
  }
}

using FluentValidation;
using MyCustomerService.Application.Dtos.Requests;

namespace MyCustomerService.API.Validators;

public class CreateCustomerValidator : AbstractValidator<AddCustomerRequestDto>
{
  public CreateCustomerValidator()
  {
    RuleFor(x => x.DriverLicenseType)
      .NotEmpty()
      .IsInEnum().WithMessage(@"Valid license are ""A"", ""B"", ""A+B""");

    RuleFor(x => x.Cnpj)
      .NotEmpty().WithMessage("CNPJ is required.");

    RuleFor(x => x.DriverLicense)
      .NotEmpty().WithMessage("Driver License is required.");
  }
}

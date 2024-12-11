using eCommerce.Core.Dto;
using FluentValidation;

namespace eCommerce.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(registerRequest => registerRequest.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(registerRequest => registerRequest.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(5).WithMessage("Password length must be minimum 5 characters");

        RuleFor(registerRequest => registerRequest.PersonName)
            .NotEmpty().WithMessage("PersonName is required")
            .Length(1, 50).WithMessage("PersonName length must be between 1 and 50 characters");

        RuleFor(registerRequest => registerRequest.Gender)
            .IsInEnum().WithMessage("Gender value must be in the enum list");
    }
}
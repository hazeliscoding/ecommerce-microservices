using eCommerce.Core.Dto;
using FluentValidation;

namespace eCommerce.Core.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(loginRequest => loginRequest.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format");

        RuleFor(loginRequest => loginRequest.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
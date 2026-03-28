using FlowDesk.Api.DTOs.Auth;
using FluentValidation;

namespace FlowDesk.Api.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.")
            .Matches(@"^FD-\d{4}-\d{4}$").WithMessage("Invalid Employee ID format. Expected: FD-YYYY-XXXX");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
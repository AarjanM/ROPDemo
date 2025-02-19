using RopDemo.Domain;
using RopDemo.SharedKernel.Requests;

namespace RopDemo.Features.CreateAccount;

public sealed record CreateAccountRequest(
    string FirstName,
    string LastName,
    string EmailAddress,
    string? TelephoneNumber) : IRequest
{
    public ValidationResult Validate() => this.Validate(ConfigureValidator);
    
    public Result WhenValid() => this.ValidWhen(ConfigureValidator);

    private static void ConfigureValidator(InlineValidator<CreateAccountRequest> validator)
    {
        validator.RuleFor(request => request.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(Account.MaxNameLength).WithMessage("First name can not exceed {MaxLength} characters.");
        validator.RuleFor(request => request.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(Account.MaxNameLength).WithMessage("Last name can not exceed {MaxLength} characters.");
        validator.RuleFor(request => request.EmailAddress)
            .NotEmpty().WithMessage("Email address is required.")
            .MaximumLength(Account.MaxEmailAddressLength).WithMessage("Email address can not exceed {MaxLength} characters.")
            .EmailAddress().WithMessage("Invalid email address.");
        validator.When(request => !string.IsNullOrEmpty(request.TelephoneNumber), () =>
        {
            validator.RuleFor(request => request.TelephoneNumber)
                .TelephoneNumber().WithMessage("Invalid telephone number.");
        });
    }
}
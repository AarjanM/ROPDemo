namespace RopDemo.Domain;

public sealed class Account : AggregateRoot<AccountId>
{
    private Account() {}

    public AccountId Id { get; private set; }
    
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string EmailAddress { get; private set; } = string.Empty;
    public TelephoneNumber TelephoneNumber { get; private set; } = TelephoneNumber.Empty();
    public DateTime RegistrationDate { get; private set; }
    
    public string DisplayName => $"{FirstName} {LastName}";
    
    public const int MaxNameLength = 50;
    public const int MaxEmailAddressLength = 200;

    public static Account CreateImperative(
        string firstName, 
        string lastName,
        string emailAddress,
        TelephoneNumber telephoneNumber,
        DateTime registrationDate)
    {
        var validationResult = (firstName, lastName, emailAddress).Validate(v =>
            {
                v.RuleFor(request => firstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(MaxNameLength)
                    .WithMessage("First name can not exceed {MaxLength} characters.");
                v.RuleFor(request => lastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(MaxNameLength)
                    .WithMessage("Last name can not exceed {MaxLength} characters.");
                v.RuleFor(request => request.emailAddress)
                    .NotEmpty().WithMessage("Email address is required.")
                    .MaximumLength(MaxEmailAddressLength)
                    .WithMessage("Email address can not exceed {MaxLength} characters.")
                    .EmailAddress().WithMessage("Invalid email address.");
            }
        );
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return new Account
        {
            Id = new AccountId(Guid.NewGuid()),
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            TelephoneNumber = telephoneNumber,
            RegistrationDate = registrationDate
        };
    }
    
    public static Result<Account> Create(
        string firstName, 
        string lastName,
        string emailAddress,
        TelephoneNumber telephoneNumber,
        DateTime registrationDate)
    {
        var validationResult = (firstName, lastName, emailAddress).ValidWhen(v =>
            {
                v.RuleFor(request => firstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(MaxNameLength)
                    .WithMessage("First name can not exceed {MaxLength} characters.");
                v.RuleFor(request => lastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(MaxNameLength)
                    .WithMessage("Last name can not exceed {MaxLength} characters.");
                v.RuleFor(request => request.emailAddress)
                    .NotEmpty().WithMessage("Email address is required.")
                    .MaximumLength(MaxEmailAddressLength)
                    .WithMessage("Email address can not exceed {MaxLength} characters.")
                    .EmailAddress().WithMessage("Invalid email address.");
            }
        );
        if (validationResult.IsFailure)
        {
            return Result.Failure<Account>(validationResult.Error);
        }

        return Result.Success(new Account
        {
            Id = new AccountId(Guid.NewGuid()),
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            TelephoneNumber = telephoneNumber,
            RegistrationDate = registrationDate
        });
    }
}

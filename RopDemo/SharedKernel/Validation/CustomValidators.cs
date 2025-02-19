using PhoneNumbers;

namespace RopDemo.SharedKernel.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string?> TelephoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidTelephoneNumber).WithMessage("Invalid telephone number.");
    }

    private static bool IsValidTelephoneNumber(string? number)
    {
        if (number == null)
        {
            return false;
        }
        
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(number, "NL");
            return phoneNumberUtil.IsValidNumber(phoneNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}
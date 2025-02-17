using PhoneNumbers;

namespace RopDemo.Domain;

public sealed record TelephoneNumber
{
    private TelephoneNumber() { }

    public required string Number { get; init; }

    public required bool IsMobile { get; init; }
    
    public static TelephoneNumber Empty() => new() { Number = String.Empty, IsMobile = false };

    public static TelephoneNumber CreateImperative(string? number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return Empty();
        }
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        PhoneNumber phoneNumber;
        try
        {
            phoneNumber = phoneNumberUtil.Parse(number, "NL");
        }
        catch (NumberParseException parseException)
        {
            throw new InvalidTelephoneNumberException(parseException);
        }

        return phoneNumberUtil.IsValidNumber(phoneNumber)
            ? new TelephoneNumber
            {
                Number = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL),
                IsMobile = phoneNumberUtil.GetNumberType(phoneNumber) == PhoneNumberType.MOBILE
            }
            : throw new InvalidTelephoneNumberException();
    }
    
    public static Result<TelephoneNumber> Create(string? number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return Empty();
        }
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        PhoneNumber phoneNumber;
        try
        {
            phoneNumber = phoneNumberUtil.Parse(number, "NL");
        }
        catch (NumberParseException)
        {
            return Errors.InvalidPhoneNumber;
        }
    
        return phoneNumberUtil.IsValidNumber(phoneNumber)
            ? new TelephoneNumber
            {
                Number = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL),
                IsMobile = phoneNumberUtil.GetNumberType(phoneNumber) == PhoneNumberType.MOBILE
            }
            : Errors.InvalidPhoneNumber;
    }


    public class InvalidTelephoneNumberException(Exception? innerException = null)
        : ApplicationException("Invalid telephone number", innerException);

    private static class Errors
    {
        public static readonly Error InvalidPhoneNumber =
            new("PhoneNumber.Invalid", "Given phone number is invalid");
    }
}
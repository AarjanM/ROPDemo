namespace RopDemo.SharedKernel.Results;

public record Error(string Code, string? Message = "")
{
    public static readonly Error None = new("");
    public static readonly Error NullValue = new("Error.NullValue");
}

public record NotFoundError(string Code, string? Message = "")
    : Error(Code, Message);
public record DomainError(string Code, string? Message = "")
    : Error(Code, Message);
public record ValidationError(string Code, Error[] Errors)
    : Error(Code, "One or more validation errors occurred.");
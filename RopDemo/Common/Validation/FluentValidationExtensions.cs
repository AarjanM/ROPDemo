namespace RopDemo.Common.Validation;

public static class FluentValidationExtensions
{
    public static ValidationResult Validate<T>(this T obj, Action<InlineValidator<T>> configure)
    {
        var validator = new InlineValidator<T>();
        configure(validator);

        var validationResult = validator.Validate(obj);

        return validationResult;
    }

    public static IResult ToProblem(this ValidationResult validationResult)
    {
        return Microsoft.AspNetCore.Http.Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://httpstatuses.com/400",
            Extensions = new Dictionary<string, object?>
                { { "errors", validationResult.Errors.Select(error => error.ErrorMessage).ToList() } }
        });
    }

    public static Result ValidWhen<T>(this T obj, Action<InlineValidator<T>> configure)
    {
        var validator = new InlineValidator<T>();
        configure(validator);

        var validationResult = validator.Validate(obj);

        return validationResult.IsValid
            ? Result.Success()
            : new ValidationError($"{typeof(T).Name}.Validation",
                    validationResult.Errors
                        .Select(failure => new Error(
                            failure.ErrorCode,
                            failure.ErrorMessage)).ToArray());
    }
}
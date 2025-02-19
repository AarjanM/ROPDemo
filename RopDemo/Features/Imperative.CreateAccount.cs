using RopDemo.Domain;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;
using RopDemo.SharedKernel.Requests;

namespace RopDemo.Features;

internal sealed class ImperativeCreateAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/imperative/accounts",
                (CreateAccountRequest request,
                    IAccountRepository repository,
                    TimeProvider timeProvider) =>
                {
                    // Validate request
                    var validationResult = request.Validate();
                    if (!validationResult.IsValid)
                    {
                        return validationResult.ToProblem();
                    }

                    // Email should be unique
                    if (!repository.IsUnique(request.EmailAddress))
                    {
                        return Results.Problem(new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Email address already exists.",
                            Type = "https://httpstatuses.com/400"
                        });
                    }

                    // Create ValueObject TelephoneNumber
                    TelephoneNumber telephoneNumber;
                    try
                    {
                        telephoneNumber = TelephoneNumber.CreateImperative(request.TelephoneNumber);
                    }
                    catch (TelephoneNumber.InvalidTelephoneNumberException)
                    {
                        return Results.Problem(new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Invalid telephone number.",
                            Type = "https://httpstatuses.com/400"
                        });
                    }

                    // Create domain object
                    Account account;
                    try
                    {
                        account = Account.CreateImperative(
                            request.FirstName,
                            request.LastName,
                            request.EmailAddress,
                            telephoneNumber,
                            timeProvider.GetUtcNow().DateTime);
                    }
                    catch (ValidationException ex)
                    {
                        return Results.Problem(new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "One or more validation errors occurred.",
                            Type = "https://httpstatuses.com/400",
                            Extensions = new Dictionary<string, object?>
                                { { "errors", ex.Errors.Select(error => error.ErrorMessage).ToList() } }
                        });
                    }

                    // Store domain object
                    repository.Add(account);

                    // SaveChanges
                    repository.SaveChanges();

                    // return CreatedAtRoute
                    return Results.CreatedAtRoute(
                        nameof(ImperativeGetAccount),
                        new { id = account.Id },
                        account);
                })
            .WithName(nameof(ImperativeCreateAccount))
            .WithDisplayName("Create an account the imperative way.")
            .WithTags("Imperative")
            .Produces<Account>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
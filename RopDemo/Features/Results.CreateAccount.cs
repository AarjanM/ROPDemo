using RopDemo.Domain;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;
using RopDemo.SharedKernel.Requests;

namespace RopDemo.Features;

internal sealed class ResultsCreateAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/results/accounts",
            (CreateAccountRequest request,
                IAccountRepository repository,
                TimeProvider timeProvider) =>
            {
                // Validate request
                var validationResult = request.WhenValid();
                if (validationResult.IsFailure)
                {
                    return ApiResults.Problem(validationResult);
                }

                // Email should be unique
                var emailAddressIsUniqueResult = WhenEmailAddressIsUnique(request.EmailAddress, repository);
                if (emailAddressIsUniqueResult.IsFailure)
                {
                    return ApiResults.Problem(emailAddressIsUniqueResult);
                }
                
                // Create ValueObject TelephoneNumber
                var telephoneNumberResult = TelephoneNumber.Create(request.TelephoneNumber);
                if(telephoneNumberResult.IsFailure)
                {
                    return ApiResults.Problem(telephoneNumberResult);
                }

                // Create domain object
                var accountResult = Account.Create(
                    request.FirstName,
                    request.LastName,
                    request.EmailAddress,
                    telephoneNumberResult.Value,
                    timeProvider.GetUtcNow().DateTime);
                if (accountResult.IsFailure)
                {
                    return ApiResults.Problem(accountResult);
                }
                
                // Store domain object
                repository.Add(accountResult.Value);
                
                // SaveChanges
                repository.SaveChanges();
                
                // return CreatedAtRoute
                return Results.CreatedAtRoute(
                    nameof(ResultsGetAccount),
                    new { id = accountResult.Value.Id },
                    accountResult.Value);
            })
            .WithName(nameof(ResultsCreateAccount))
            .WithDisplayName("Create an account the imperative without exceptions way.")
            .WithTags("Results Imperative")
            .Produces<Account>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private Result WhenEmailAddressIsUnique(string emailAddress, IAccountRepository repository)
    {
        return repository.IsUnique(emailAddress)
            ? Result.Success()
            : new DomainError("Account.EmailAddressNotUnique", "Email address already exists.");
    }
}
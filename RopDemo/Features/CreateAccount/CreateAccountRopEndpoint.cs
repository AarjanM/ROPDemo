using RopDemo.Domain;
using RopDemo.Features.GetAccount;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;

namespace RopDemo.Features.CreateAccount;

internal sealed class CreateAccountRopEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/rop/accounts",
                (CreateAccountRequest request,
                        IAccountRepository repository,
                        TimeProvider timeProvider)
                    =>
                    // Validate request
                    request.WhenValid()
                        // Email should be unique
                        .Bind(() => WhenEmailAddressIsUnique(request.EmailAddress, repository))
                        // Create ValueObject TelephoneNumber
                        .Bind(() => TelephoneNumber.Create(request.TelephoneNumber))
                        // Create domain object
                        .Bind(telephoneNumber => Account.Create(
                            request.FirstName,
                            request.LastName,
                            request.EmailAddress,
                            telephoneNumber,
                            timeProvider.GetUtcNow().DateTime))
                        // Store domain object
                        .Tap(repository.Add)
                        // SaveChanges;
                        .Tap(_ => repository.SaveChanges())
                        .Match(
                            // return CreatedAtRoute
                            account => Microsoft.AspNetCore.Http.Results.CreatedAtRoute(
                                nameof(GetAccountRopEndpoint),
                                new { id = account.Id },
                                account),
                            ApiResults.Problem))
            .WithName(nameof(CreateAccountRopEndpoint))
            .WithDisplayName("Create an account the railway oriented way.")
            .WithTags("Railway oriented")
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
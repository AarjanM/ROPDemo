using RopDemo.Domain;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;

namespace RopDemo.Features;

internal sealed class ImperativeGetAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/imperative/accounts/{id:guid}", 
                (Guid id, IAccountRepository repository) =>
                {
                    var account = repository.GetAccountOrNull(new AccountId(id));
                    
                    return account is not null
                        ? Results.Ok(account)
                        : Results.NotFound();
                })
            .WithName(nameof(ImperativeGetAccount))
            .WithDisplayName("Get an account the imperative way.")
            .WithTags("Imperative")
            .Produces<Account>()
            .Produces(StatusCodes.Status404NotFound);
    }
}
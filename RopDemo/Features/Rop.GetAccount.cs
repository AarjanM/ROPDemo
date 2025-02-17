using RopDemo.Domain;
using RopDemo.Services.Data;

namespace RopDemo.Features;

public class RopGetAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/rop/accounts/{id:guid}",
                (Guid id, IAccountRepository repository)
                    =>
                    repository.GetAccount(new AccountId(id))
                        .Match(
                            account => Results.Ok(account),
                            result => ApiResults.Problem(result)))
            .WithName(nameof(RopGetAccount))
            .WithDisplayName("Get an account the railway oriented way.")
            .WithTags("Railway oriented")
            .Produces<Account>()
            .Produces(StatusCodes.Status404NotFound);
    }
}
using RopDemo.Domain;
using RopDemo.Services.Data;

namespace RopDemo.Features;

public class GetAccounts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/accounts",
                (IAccountRepository repository) =>
                    Results.Ok(repository.GetAccounts()))
            .WithName(nameof(GetAccounts))
            .Produces<List<Account>>();
    }
}
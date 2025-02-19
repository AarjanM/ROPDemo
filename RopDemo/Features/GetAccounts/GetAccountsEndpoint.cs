using RopDemo.Domain;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;

namespace RopDemo.Features.GetAccounts;

public class GetAccountsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/accounts",
                (IAccountRepository repository) =>
                    Results.Ok(repository.GetAccounts()))
            .WithName(nameof(GetAccountsEndpoint))
            .Produces<List<Account>>();
    }
}
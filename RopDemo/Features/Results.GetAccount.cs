using RopDemo.Domain;
using RopDemo.Intrastructure.Data;
using RopDemo.SharedKernel.Endpoints;

namespace RopDemo.Features;

internal sealed class ResultsGetAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/results/accounts/{id:guid}", 
                (Guid id, IAccountRepository repository) =>
                {
                    var accountResult = repository.GetAccount(new AccountId(id));
                    
                    return accountResult.IsSuccess
                        ? Results.Ok(accountResult.Value)
                        : ApiResults.Problem(accountResult.Error);
                })
            .WithName(nameof(ResultsGetAccount))
            .WithDisplayName("Get an account the imperative without exceptions way.")
            .WithTags("Results Imperative")
            .Produces<Account>()
            .Produces(StatusCodes.Status404NotFound);
    }
}
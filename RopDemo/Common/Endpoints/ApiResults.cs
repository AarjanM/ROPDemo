namespace RopDemo.Common.Endpoints;

public static class ApiResults
{
    public static IResult Problem(Result result)
    {
        return result.IsFailure
            ? Microsoft.AspNetCore.Http.Results.Problem(
                statusCode: GetStatusCode(),
                title: result.Error.Code,
                detail: result.Error.Message,
                extensions: CreateExtensions())
            : throw new InvalidOperationException();

        int GetStatusCode()
        {
            return result.Error! switch
            {
                ValidationError => StatusCodes.Status400BadRequest,
                NotFoundError => StatusCodes.Status404NotFound,
                DomainError => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        
        IDictionary<string,object?>? CreateExtensions()
        {
            return result.Error switch
            {
                ValidationError error =>
                    new Dictionary<string, object?>{ { "validation.errors", error.Errors.Select(e => e.Message) }},
                _ => null
            };
        }
    }
}
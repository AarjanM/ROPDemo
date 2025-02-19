namespace RopDemo.SharedKernel.Results;

public static class ResultMapExtensions
{
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Error);
    }

    public static Result<TOut> Map<TOut>(
        this Result result,
        Func<TOut> mappingFunc)
    {
        return result.IsSuccess
            ? Result.Success(mappingFunc())
            : Result.Failure<TOut>(result.Error);
    }
}

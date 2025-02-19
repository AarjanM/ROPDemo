namespace RopDemo.SharedKernel.Results;

public static class ResultMatchExtensions
{
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result, TOut> onFailure)
        =>
        result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result);
}
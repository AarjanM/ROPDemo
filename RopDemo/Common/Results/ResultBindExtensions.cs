namespace RopDemo.Common.Results;

public static class ResultBindExtensions
{
    public static Result Bind(
        this Result result,
        Func<Result> func)
        =>
            result.IsSuccess
                ? func()
                : Result.Failure(result.Error);

    public static Result<TOut> Bind<TOut>(
        this Result result,
        Func<Result<TOut>> func)
        =>
            result.IsSuccess
                ? func()
                : Result.Failure<TOut>(result.Error);

    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func)
        =>
            result.IsSuccess
                ? func(result.Value)
                : Result.Failure<TOut>(result.Error);
}
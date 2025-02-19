namespace RopDemo.SharedKernel.Results;

public static class ResultCombineExtensions
{
    public static Result<(TIn1, TIn2)> Combine<TIn1, TIn2>(
        this Result<TIn1> result1,
        Result<TIn2> result2)
        =>
            result1.IsSuccess && result2.IsSuccess
                ? Result.Success((result1.Value, result2.Value))
                : Result.Failure<(TIn1, TIn2)>(result1.IsFailure ? result1.Error : result2.Error);
}

namespace TechDemo.Domain.Shared.Results;

public class Result<T>
{
    private readonly T? _value;

    public Result(T? value, bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        _value = value;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("There is no value to access.");

    public Error Error { get; }

    public static Result<T> Success(T value) => new(value, true, Error.None);

    public static Result<T> Failure(Error error) => new(default, false, error);

    // public Result<T> Then(Func<Result<T>> next) => IsSuccess ? next() : this;

    // public Result<TResult> Then<TResult>(Func<Result<TResult>> next) => IsSuccess
    //     ? next()
    //     : Result<TResult>.Failure(Error);

    public Result<TResult> Map<TResult>(Func<T, Result<TResult>> func) => IsSuccess
        ? func(Value)
        : Result<TResult>.Failure(Error);

    public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<Result<TResult>>> func)
    => IsSuccess
        ? await func(Value)
        : Result<TResult>.Failure(Error);

    public async Task<Result<TResult>> MapAsync<TResult>(Func<Task<Result<TResult>>> func)
    => IsSuccess
        ? await func()
        : Result<TResult>.Failure(Error);


    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    => IsSuccess
        ? onSuccess()
        : onFailure(Error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    => IsSuccess
        ? onSuccess(Value)
        : onFailure(Error);


    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    => IsSuccess
            ? await onSuccess(Value)
            : await onFailure(Error);


    public Result<TResult> Project<TResult>(Func<T, TResult> mapper) => IsSuccess
        ? Result<TResult>.Success(mapper(Value))
        : Result<TResult>.Failure(Error);

    public async Task<Result<TResult>> ProjectAsync<TResult>(Func<T, Task<TResult>> mapper)
    => IsSuccess
            ? Result<TResult>.Success(await mapper(Value))
            : Result<TResult>.Failure(Error);


    public static implicit operator Result<T>(T? value) => value is not null
        ? Success(value)
        : Failure(Error.NullValue);
}

public static class Result
{
    public static Result<Empty> Success() => new(Empty.Value, true, Error.None);

    public static Result<Empty> Failure(Error error) => new(Empty.Value, false, error);
}

public struct Empty
{
    public static readonly Empty Value = new Empty();
}

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(nameof(Error), "The value cannot be null.");
}

public static class TaskExtensions
{
    public static Result<T> Resolve<T>(this Task<Result<T>> task) => task.Result;
}
namespace TechDemo.Domain.Shared.Results;

public class Result<T>
{
    private readonly T? _value;

    public Result(T? value, bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid state.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        _value = value;
    }

    public Result(T? value, bool isSuccess, List<Error> errors)
    {
        if (isSuccess && errors.Any() ||
            !isSuccess && !errors.Any())
        {
            throw new ArgumentException("Invalid state.", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = errors;
        _value = value;
    }

    public Error Error { get; }

    public List<Error>? Errors { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("No value to was founds.");

    public static Result<T> Success(T value) => new(value, true, Error.None);

    public static Result<T> Failure(Error error) => new(default, false, error);

    public static Result<T> ValidationFailures(List<Error> errors) => new(default, false, errors);

    public Result<TResult> Then<TResult>(Func<T, Result<TResult>> func) => IsSuccess
        ? func(Value)
        : Result<TResult>.Failure(Error);

    public async Task<Result<TResult>> ThenAsync<TResult>(Func<T, Task<Result<TResult>>> func)
    => IsSuccess
        ? await func(Value)
        : Result<TResult>.Failure(Error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    => IsSuccess
        ? onSuccess(Value)
        : onFailure(Error);

    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    => IsSuccess
            ? await onSuccess(Value)
            : await onFailure(Error);


    public Result<TResult> Map<TResult>(Func<T, TResult> mapper) => IsSuccess
        ? Result<TResult>.Success(mapper(Value))
        : Result<TResult>.Failure(Error);

    public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> mapper)
    => IsSuccess
            ? Result<TResult>.Success(await mapper(Value))
            : Result<TResult>.Failure(Error);

    public Task<Result<T>> Async() => Task.FromResult(this);

    public static implicit operator Result<T>(T? value) => value is not null
        ? Success(value)
        : Failure(Error.NullValue);
}

public static class Result
{
    public static Result<None> Success() => new(None.Value, true, Error.None);

    public static Result<None> Failure(Error error) => new(None.Value, false, error);

    public static Result<None> ValidationFailures(List<Error> errors) => new(None.Value, false, errors);
}

public struct None
{
    public static readonly None Value = new None();
}

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(nameof(Error), "The value cannot be null.");
}

public static class TaskExtensions
{
    public static Result<T> Unwrap<T>(this Task<Result<T>> task) => task.Result;
}
namespace StarterKit.Maui.Core.Domain.Models;

public abstract record Result<T>
{
    public bool IsSuccess { get; }

    protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}

public abstract record Result
{
    public bool IsSuccess { get; }

    protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}

public sealed record Success : Result
{
    public Success() : base(true)
    {
    }
}

public sealed record Success<T> : Result<T>
{
    public T Value { get; }

    public Success(T value) : base(true)
    {
        Value = value;
    }
}

public sealed record Failure<T> : Result<T>
{
    public Exception Exception { get; }

    public Failure(Exception exception) : base(false)
    {
        Exception = exception;
    }
}

public sealed record Failure : Result
{
    public Exception Exception { get; }

    public Failure(Exception exception) : base(false)
    {
        Exception = exception;
    }
}
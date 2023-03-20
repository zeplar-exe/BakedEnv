namespace BakedEnv.Common;

public class OperationResult<T>
{
    public T Value { get; }
    public bool HasError { get; }
    public BakedError Error { get; }

    public static OperationResult<T> Success(T expression)
    {
        return new OperationResult<T>(expression, false, default);
    }
    
    public static OperationResult<T> Failure(BakedError error)
    {
        return new OperationResult<T>(default, true, error);
    }

    protected OperationResult(T value, bool hasError, BakedError error)
    {
        Value = value;
        HasError = hasError;
        Error = error;
    }
}
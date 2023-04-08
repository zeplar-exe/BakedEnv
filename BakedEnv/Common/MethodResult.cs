namespace BakedEnv.Common;

public class MethodResult<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }

    public static MethodResult<T> Success(T expression)
    {
        return new MethodResult<T>(expression, true);
    }
    
    public static MethodResult<T> Failure()
    {
        return new MethodResult<T>(default, false);
    }

    protected MethodResult(T value, bool success)
    {
        Value = value;
        IsSuccess = success;
    }
}
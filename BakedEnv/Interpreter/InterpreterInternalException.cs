namespace BakedEnv.Interpreter;

internal class InterpreterInternalException : Exception
{
    public BakedError[] Errors { get; }

    public InterpreterInternalException(BakedError error)
    {
        Errors = new[] { error };
    }
    
    public InterpreterInternalException(BakedError[] errors)
    {
        Errors = errors;
    }
}
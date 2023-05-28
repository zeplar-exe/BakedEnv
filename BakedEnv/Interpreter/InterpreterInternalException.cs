namespace BakedEnv.Interpreter;

internal class InterpreterInternalException : Exception
{
    public BakedError Error { get; }

    public InterpreterInternalException(BakedError error)
    {
        Error = error;
    }
}
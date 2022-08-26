namespace BakedEnv.Interpreter.Scopes;

public record InvocationContext(BakedInterpreter Interpreter, IBakedScope Scope, ulong SourceIndex = 0)
{
    public InvocationContext(BakedInterpreter interpreter, ulong sourceIndex = 0) : this(interpreter, interpreter.Context, sourceIndex)
    {
        
    }

    public void ReportError(BakedError error)
    {
        Interpreter.ReportError(error);
    }
}
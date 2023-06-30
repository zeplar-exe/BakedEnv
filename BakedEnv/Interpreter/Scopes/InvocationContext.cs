namespace BakedEnv.Interpreter.Scopes;

public record InvocationContext(BakedInterpreter Interpreter, IBakedScope Scope, long SourceIndex = 0)
{
    public InvocationContext(BakedInterpreter interpreter, long sourceIndex = 0) : this(interpreter, interpreter.Context, sourceIndex)
    {
        
    }

    public void ReportError(BakedError error)
    {
        Interpreter.Error.Report(error);
    }
}
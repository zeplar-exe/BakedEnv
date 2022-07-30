namespace BakedEnv.Interpreter.Scopes;

public record InvocationContext(BakedInterpreter Interpreter, IBakedScope Scope, int SourceIndex = -1)
{
    public InvocationContext(BakedInterpreter interpreter, int sourceIndex = -1) : this(interpreter, interpreter.Context, sourceIndex)
    {
        
    }

    public void ReportError(BakedError error)
    {
        Interpreter.ReportError(error);
    }
}
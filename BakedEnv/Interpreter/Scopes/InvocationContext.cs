namespace BakedEnv.Interpreter.Scopes;

public record InvocationContext(BakedInterpreter Interpreter, IBakedScope Scope, int SourceIndex = -1)
{
    public InvocationContext(BakedInterpreter interpreter, int SourceIndex = -1) : this(interpreter, interpreter.Context, SourceIndex)
    {
        
    }
}
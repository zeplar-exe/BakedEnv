using BakedEnv.Variables;

namespace BakedEnv.Interpreter.Scopes;

public class EmptyScope : IBakedScope
{
    public IBakedScope? Parent { get; }
    public VariableContainer Variables { get; }

    public EmptyScope()
    {
        Parent = null;
        Variables = new VariableContainer();
    }
}
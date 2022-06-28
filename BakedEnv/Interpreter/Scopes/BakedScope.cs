using BakedEnv.Interpreter.Variables;

namespace BakedEnv.Interpreter.Scopes;

public class BakedScope : IBakedScope
{
    public IBakedScope? Parent { get; }
    public VariableContainer Variables { get; }
    
    public BakedScope(IBakedScope? parent)
    {
        Parent = parent;
        Variables = new VariableContainer();
    }
}
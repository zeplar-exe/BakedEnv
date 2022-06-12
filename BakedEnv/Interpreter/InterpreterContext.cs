using BakedEnv.Objects;

namespace BakedEnv.Interpreter;

public class InterpreterContext : IBakedScope
{
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedObject> Variables { get; }

    internal InterpreterContext()
    {
        Parent = null;
        Variables = new Dictionary<string, BakedObject>();
    }
}
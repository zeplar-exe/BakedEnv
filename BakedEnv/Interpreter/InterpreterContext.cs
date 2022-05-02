using BakedEnv.Objects;

namespace BakedEnv.Interpreter;

public class InterpreterContext : IBakedScope
{
    public BakeType BakeType { get; set; }
    
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedObject> Variables { get; }

    internal InterpreterContext()
    {
        BakeType = BakeType.Script;
        Parent = null;
        Variables = new Dictionary<string, BakedObject>();
    }
}
using BakedEnv.Objects;

namespace BakedEnv.Interpreter;

public class InterpreterContext : IBakedScope
{
    public BakeType BakeType { get; set; }
    
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedObject> Variables { get; }
    public Dictionary<string, BakedMethod> Methods { get; }

    internal InterpreterContext()
    {
        BakeType = BakeType.Script;
        Parent = null;
        Variables = new Dictionary<string, BakedObject>();
        Methods = new Dictionary<string, BakedMethod>();
    }
}
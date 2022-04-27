namespace BakedEnv.Interpreter;

public class InterpreterContext : IBakedScope
{
    public BakeType BakeType { get; set; }
    public bool NullReferenceErrorEnabled { get; set; }
    
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedVariable> Variables { get; }
    public Dictionary<string, BakedMethod> Methods { get; }

    internal InterpreterContext()
    {
        BakeType = BakeType.Script;
        Parent = null;
        Variables = new Dictionary<string, BakedVariable>();
        Methods = new Dictionary<string, BakedMethod>();
    }
}
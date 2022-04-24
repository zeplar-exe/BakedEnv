namespace BakedEnv.Interpreter;

public class InterpreterContext : IBakedScope
{
    public BakeType BakeType { get; set; }
    
    public IBakedScope? Parent { get; }
    public List<BakedVariable> Variables { get; }
    public List<BakedMethod> Methods { get; }

    internal InterpreterContext()
    {
        BakeType = BakeType.Script;
        Parent = null;
        Variables = new List<BakedVariable>();
        Methods = new List<BakedMethod>();
    }
}
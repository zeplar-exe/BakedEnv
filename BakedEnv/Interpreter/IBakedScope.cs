namespace BakedEnv.Interpreter;

public interface IBakedScope
{
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedVariable> Variables { get; }
    public Dictionary<string, BakedMethod> Methods { get; }
}
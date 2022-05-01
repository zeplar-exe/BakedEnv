using BakedEnv.Objects;

namespace BakedEnv.Interpreter;

public interface IBakedScope
{
    public IBakedScope? Parent { get; }
    public Dictionary<string, BakedObject> Variables { get; }
    public Dictionary<string, BakedMethod> Methods { get; }
}
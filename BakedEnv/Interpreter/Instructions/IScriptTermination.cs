using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public interface IScriptTermination
{
    public BakedObject ReturnValue { get; }
}
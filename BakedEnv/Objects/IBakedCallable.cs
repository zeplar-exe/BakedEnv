using BakedEnv.Interpreter;

namespace BakedEnv.Objects;

public interface IBakedCallable
{
    public BakedObject Invoke(BakedObject[] parameters, BakedInterpreter interpreter, IBakedScope scope);
}
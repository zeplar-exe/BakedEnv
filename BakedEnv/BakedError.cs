using BakedEnv.Interpreter;

namespace BakedEnv;

public partial record struct BakedError
{
    internal void Throw()
    {
        throw new InterpreterInternalException(this);
    }
}
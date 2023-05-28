using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;

namespace BakedEnv;

public partial record struct BakedError
{
    internal void Throw()
    {
        throw new InterpreterInternalException(this);
    }
}
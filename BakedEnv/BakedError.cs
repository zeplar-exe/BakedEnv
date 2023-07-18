using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter;

namespace BakedEnv;

public partial record struct BakedError
{
    [DoesNotReturn]
    internal Exception Throw()
    {
        throw new InterpreterInternalException(this);
    }
}
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public abstract class BakedExpression
{
    public abstract BakedObject Evaluate(BakedInterpreter interpreter, InvocationContext context);
}
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class NullExpression : BakedExpression
{
    public override BakedObject Evaluate(InvocationContext context)
    {
        return new BakedNull();
    }
}
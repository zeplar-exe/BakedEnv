using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public abstract class BakedExpression
{
    public abstract BakedObject Evaluate(InvocationContext context);
    public virtual bool TryAssign(BakedObject value, InvocationContext context) => false;

    protected static bool TryAssignForExpression(BakedExpression expression, BakedObject value, InvocationContext context)
    {
        if (expression is not VariableExpression variableExpression)
            return false;

        return variableExpression.TryAssign(value, context);
    }
}
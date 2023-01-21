using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Expressions;

public interface IAssignableExpression
{
    public bool TryAssign(BakedExpression other, InvocationContext context);
}
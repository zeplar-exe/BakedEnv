using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class ParenthesisExpression : BakedExpression
{
    public BakedExpression Expression { get; }

    public ParenthesisExpression(BakedExpression expression)
    {
        Expression = expression;
    }
    
    public override BakedObject Evaluate(InvocationContext context)
    {
        return Expression.Evaluate(context);
    }
    
    public override bool TryAssign(BakedObject value, InvocationContext context)
    {
        return TryAssignForExpression(Expression, value, context);
    }
}
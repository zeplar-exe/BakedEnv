using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class NegateExpression : BakedExpression
{
    public BakedExpression Expression { get; }
    
    public NegateExpression(BakedExpression expression)
    {
        Expression = expression;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var value = Expression.Evaluate(context);
        
        if (!value.TryNegate(out var negated))
        {
            context.ReportError(BakedError.VAL.E1001("negate", value.TypeName(), context.SourceIndex));
        }

        return negated;
    }
    
    public override bool TryAssign(BakedObject value, InvocationContext context)
    {
        return TryAssignForExpression(Expression, value, context);
    }
}
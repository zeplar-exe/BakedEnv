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
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvalidOperator,
                ErrorMessages.InvalidUnaryOperation("negate", value),
                context.SourceIndex));
        }

        return negated;
    }
}
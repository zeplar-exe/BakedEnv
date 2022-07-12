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
        if (Expression.Evaluate(context).TryNegate(out var negated))
        {
            return negated;
        }
        else
        {
            context.Interpreter.ReportError(new BakedError()); // TODO
        }

        return new BakedNull();
    }
}
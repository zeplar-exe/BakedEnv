using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class InvocationExpression : BakedExpression
{
    public BakedExpression Expression { get; }
    public BakedExpression[] Parameters { get; }

    public InvocationExpression(BakedExpression expression, BakedExpression[] parameters)
    {
        Expression = expression;
        Parameters = parameters;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var parameters = Parameters.Select(p => p.Evaluate(context)).ToArray();
        var value = Expression.Evaluate(context);

        if (value is BakedNull)
        {
            context.ReportError(BakedEnv.BakedError.INVK.E1000(context.SourceIndex));

            return value;
        }

        if (value is not IBakedCallable callable)
        {
            context.ReportError(BakedEnv.BakedError.INVK.E1001(context.SourceIndex));
            
            return new BakedNull();
        }

        return callable.Invoke(parameters, context);
    }
}
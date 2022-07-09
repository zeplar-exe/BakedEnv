using BakedEnv.Interpreter.Instructions;
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
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvokeNull,
                "Cannot invoke a null value.",
                context.SourceIndex));

            return value;
        }

        if (value is not IBakedCallable callable)
        {
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvokeNonCallable,
                "Cannot invoke a non-callable object.",
                context.SourceIndex));
            
            return new BakedNull();
        }

        return callable.Invoke(parameters, context);
    }
}
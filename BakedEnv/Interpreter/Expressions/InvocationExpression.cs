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
            context.ReportError(BakedError.ENullInvocation(context.SourceIndex));

            return value;
        }

        if (value is not IBakedCallable callable)
        {
            context.ReportError(BakedError.ENonCallableInvocation(context.SourceIndex));
            
            return new BakedNull();
        }

        return callable.Invoke(parameters, context);
    }
    
    public override bool TryAssign(BakedObject value, InvocationContext context)
    {
        return TryAssignForExpression(Expression, value, context);
    }
}
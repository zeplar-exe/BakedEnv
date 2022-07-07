using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class InvocationExpression : BakedExpression
{
    public IBakedCallable Callable { get; }
    public BakedExpression[] Parameters { get; }

    public InvocationExpression(IBakedCallable callable, BakedExpression[] parameters)
    {
        Callable = callable;
        Parameters = parameters;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var parameters = Parameters.Select(p => p.Evaluate(context)).ToArray();
        
        return Callable.Invoke(parameters, context);
    }
}
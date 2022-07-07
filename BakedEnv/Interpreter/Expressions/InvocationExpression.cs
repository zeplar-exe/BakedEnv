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

    public override BakedObject Evaluate(BakedInterpreter interpreter, InvocationContext context)
    {
        var parameters = Parameters.Select(p => p.Evaluate(interpreter, context)).ToArray();
        
        return Callable.Invoke(parameters, interpreter, context);
    }
}
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class InvocationExpression : BakedExpression
{
    public IBakedCallable Callable { get; }
    public BakedObject[] Parameters { get; }

    public InvocationExpression(IBakedCallable callable, BakedObject[] parameters)
    {
        Callable = callable;
        Parameters = parameters;
    }

    public override BakedObject Evaluate(BakedInterpreter interpreter, InvocationContext context)
    {
        return Callable.Invoke(Parameters, interpreter, context);
    }
}
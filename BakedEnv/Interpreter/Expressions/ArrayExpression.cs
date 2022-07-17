using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class ArrayExpression : BakedExpression
{
    private BakedExpression[] Expressions { get; }

    public ArrayExpression(IEnumerable<BakedExpression> expressions)
    {
        Expressions = expressions.ToArray();
    }
    
    public override BakedObject Evaluate(InvocationContext context)
    {
        return new BakedArray(Expressions.Select(e => e.Evaluate(context)));
    }
}
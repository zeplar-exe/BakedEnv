using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class ExponentiationExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public ExponentiationExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TryExponent(Right.Evaluate(context), out var result))
        {
            context.ReportError(BakedError.VAL.E1000(
                "raise", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
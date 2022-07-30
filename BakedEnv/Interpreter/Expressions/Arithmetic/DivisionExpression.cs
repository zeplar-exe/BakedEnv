using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class DivisionExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public DivisionExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TryDivide(Right.Evaluate(context), out var result))
        {
            context.ReportError(BakedEnv.BakedError.VAL.E1000(
                "divide", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
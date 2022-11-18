using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class SubtractionExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public SubtractionExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TrySubtract(Right.Evaluate(context), out var result))
        {
            context.ReportError(BakedError.EInvalidBinaryOperation(
                "subtract", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
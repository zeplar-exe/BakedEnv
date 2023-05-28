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
        
        if (!left.TryDivide(right, out var result) && !right.TryDivide(left, out result))
        {
            context.ReportError(BakedError.EInvalidBinaryOperation(
                "divide", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
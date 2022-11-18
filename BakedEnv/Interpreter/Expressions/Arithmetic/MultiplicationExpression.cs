using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class MultiplicationExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public MultiplicationExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TryMultiply(Right.Evaluate(context), out var result))
        {
            context.ReportError(BakedError.EInvalidBinaryOperation(
                "multiply", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
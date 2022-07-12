using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class AdditionExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public AdditionExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TryAdd(Right.Evaluate(context), out var result))
        {
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvalidOperator,
                ErrorMessages.InvalidBinaryOperation("add", left, right),
                context.SourceIndex));
        }

        return result;
    }
}
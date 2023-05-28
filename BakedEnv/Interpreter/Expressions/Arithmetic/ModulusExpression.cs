using BakedEnv.Extensions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions.Arithmetic;

public class ModulusExpression : BakedExpression
{
    public BakedExpression Left { get; }
    public BakedExpression Right { get; }
    
    public ModulusExpression(BakedExpression left, BakedExpression right)
    {
        Left = left;
        Right = right;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);
        
        if (!left.TryModulus(right, out var result) && !right.TryModulus(left, out result))
        {
            context.ReportError(BakedError.EInvalidBinaryOperation(
                "modulo", 
                left.TypeName(), right.TypeName(),
                context.SourceIndex));
        }

        return result;
    }
}
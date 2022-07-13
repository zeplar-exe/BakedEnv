using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class IndexExpression : BakedExpression
{
    public BakedExpression Target { get; }
    public BakedExpression Value { get; }
    
    public IndexExpression(BakedExpression target, BakedExpression value)
    {
        Target = target;
        Value = value;
    }
    
    public override BakedObject Evaluate(InvocationContext context)
    {
        var target = Target.Evaluate(context);
        var value = Value.Evaluate(context);
        
        if (!target.TryGetIndex(value, out var output))
        {
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvalidOperator,
                ErrorMessages.InvalidIndex(target, value),
                context.SourceIndex));
        }

        return output;
    }
}
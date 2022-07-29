using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class IndexExpression : BakedExpression
{
    public BakedExpression Target { get; }
    public BakedExpression[] Values { get; }
    
    public IndexExpression(BakedExpression target, IEnumerable<BakedExpression> values)
    {
        Target = target;
        Values = values.ToArray();
    }
    
    public override BakedObject Evaluate(InvocationContext context)
    {
        var target = Target.Evaluate(context);
        var values = Values.Select(v => v.Evaluate(context)).ToArray();
        
        if (!target.TryGetIndex(values.ToArray(), out var output))
        {
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvalidOperator,
                ErrorMessages.InvalidIndex(target, values),
                context.SourceIndex));
        }

        return output;
    }
}
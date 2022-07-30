using BakedEnv.Helpers;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class VariableExpression : BakedExpression
{
    public VariableExpression(VariableReference reference)
    {
        Reference = reference;
    }

    public VariableReference Reference { get; }

    public override BakedObject Evaluate(InvocationContext context)
    {
        if (!Reference.TryGetVariable(out var variable))
        {
            if (Reference.IsLocal())
            {
                context.ReportError(BakedError.VAR.E1000(Reference.Name, context.SourceIndex));
            }
            
            context.ReportError(BakedError.VAR.E1001(
                string.Join('.', Reference.FullPath),
                context.SourceIndex));
        }
        
        return variable?.Value ?? new BakedNull();
    }
}
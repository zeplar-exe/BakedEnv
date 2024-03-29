using BakedEnv.Helpers;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.Expressions;

public class VariableExpression : BakedExpression, IAssignableExpression
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
                context.ReportError(BakedError.EInvalidLocalVariable(Reference.Name, context.SourceIndex));
            }
            
            context.ReportError(BakedError.EInvalidVariablePath(
                string.Join('.', Reference.FullPath),
                context.SourceIndex));
        }
        
        return variable?.Value ?? new BakedNull();
    }

    public bool TryAssign(BakedExpression other, InvocationContext context)
    {
        return Reference.TrySetVariable(other.Evaluate(context));
    }
}
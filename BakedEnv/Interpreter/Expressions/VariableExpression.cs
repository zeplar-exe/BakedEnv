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
            context.Interpreter.ReportError(new BakedError(
                ErrorCodes.InvalidVariableOrPath, 
                $"Variable, variable path, or part of path " + 
                $"'{string.Join(".", Reference.Path.AsEnumerable())}' does not exist.",
                context.SourceIndex));
        }
        
        return variable?.Value ?? new BakedNull();
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// Instruction to invoke an <see cref="IBakedCallable"/>.
/// </summary>
public class ObjectInvocationInstruction : InterpreterInstruction
{
    /// <summary>
    /// Callable object to invoke.
    /// </summary>
    public BakedExpression Expression { get; set; }
    /// <summary>
    /// NameList to use during invocation.
    /// </summary>
    public BakedExpression[] Parameters { get; set; }

    /// <summary>
    /// Initialize an ObjectInvocationInstruction.
    /// </summary>
    /// <param name="callable">Callable object to invoke.</param>
    /// <param name="parameters">NameList to use during invocation.</param>
    /// /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public ObjectInvocationInstruction(BakedExpression expression, BakedExpression[] parameters, int sourceIndex) : base(sourceIndex)
    {
        Expression = expression;
        Parameters = parameters;
    }

    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        ExecuteReturn(context);
    }

    public BakedObject ExecuteReturn(InvocationContext context)
    {
        var parameters = Parameters.Select(p => p.Evaluate(context)).ToArray();
        var value = Expression.Evaluate(context);

        if (value is BakedNull)
        {
            context.ReportError(BakedEnv.BakedError.INVK.E1000(context.SourceIndex));

            return value;
        }

        if (value is not IBakedCallable callable)
        {
            context.ReportError(BakedEnv.BakedError.INVK.E1001(context.SourceIndex));
            
            return new BakedNull();
        }
        
        return callable.Invoke(parameters, context);
    }
}
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
    public IBakedCallable Callable { get; set; }
    /// <summary>
    /// Parameters to use during invocation.
    /// </summary>
    public BakedExpression[] Parameters { get; set; }

    /// <summary>
    /// Initialize an ObjectInvocationInstruction.
    /// </summary>
    /// <param name="callable">Callable object to invoke.</param>
    /// <param name="parameters">Parameters to use during invocation.</param>
    /// /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public ObjectInvocationInstruction(IBakedCallable callable, BakedExpression[] parameters, int sourceIndex) : base(sourceIndex)
    {
        Callable = callable;
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
        
        return Callable.Invoke(parameters, context);
    }
}
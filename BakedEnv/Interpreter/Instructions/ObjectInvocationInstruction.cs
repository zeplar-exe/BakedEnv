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
    public BakedObject[] Parameters { get; set; }

    /// <summary>
    /// Initialize an ObjectInvocationInstruction.
    /// </summary>
    /// <param name="callable">Callable object to invoke.</param>
    /// <param name="parameters">Parameters to use during invocation.</param>
    /// /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public ObjectInvocationInstruction(IBakedCallable callable, BakedObject[] parameters, int sourceIndex = -1) : base(sourceIndex)
    {
        Callable = callable;
        Parameters = parameters;
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        Callable.Invoke(Parameters, interpreter, scope);
    }
}
using System.Reflection;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// Instruction to invoke a raw <see cref="MethodInfo"/>.
/// </summary>
public class MethodInfoInstruction : InterpreterInstruction
{
    /// <summary>
    /// The method target object.
    /// </summary>
    public object Target { get; set; }
    /// <summary>
    /// The MethodInfo to execute.
    /// </summary>
    public MethodInfo MethodInfo { get; set; }
    /// <summary>
    /// Any parameters to use during invocation.
    /// </summary>
    public object?[] Parameters { get; set; }

    /// <summary>
    /// Initialize a MethodInfoInstruction with its target, method, and parameters.
    /// </summary>
    /// <param name="target">The method target object.</param>
    /// <param name="methodInfo">The MethodInfo to execute.</param>
    /// <param name="parameters">Any parameters to use during invocation.</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public MethodInfoInstruction(object target, MethodInfo methodInfo, object?[] parameters, int sourceIndex) : base(sourceIndex)
    {
        Target = target;
        MethodInfo = methodInfo;
        Parameters = parameters;
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        MethodInfo.Invoke(Target, Parameters);
    }
}
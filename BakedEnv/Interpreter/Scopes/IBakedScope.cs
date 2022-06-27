using BakedEnv.Interpreter.Variables;

namespace BakedEnv.Interpreter;

/// <summary>
/// A layered scope used in execution.
/// </summary>
public interface IBakedScope
{
    /// <summary>
    /// The parent scope, if any.
    /// </summary>
    public IBakedScope? Parent { get; }
    /// <summary>
    /// Any variables specific to this scope.
    /// </summary>
    public VariableContainer Variables { get; }
}
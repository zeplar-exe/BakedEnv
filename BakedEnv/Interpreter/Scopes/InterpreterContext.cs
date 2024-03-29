using BakedEnv.Variables;

namespace BakedEnv.Interpreter.Scopes;

/// <summary>
/// The top-level scope of a <see cref="BakedInterpreter"/>.
/// </summary>
public class InterpreterContext : IBakedScope
{
    /// <inheritdoc />
    public IBakedScope? Parent { get; }

    /// <inheritdoc />
    public VariableContainer Variables { get; }

    internal InterpreterContext()
    {
        Parent = null;
        Variables = new VariableContainer();
    }
}
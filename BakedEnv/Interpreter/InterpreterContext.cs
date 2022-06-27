using BakedEnv.Interpreter.Variables;

namespace BakedEnv.Interpreter;

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
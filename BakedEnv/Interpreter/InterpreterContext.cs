using BakedEnv.Objects;

namespace BakedEnv.Interpreter;

/// <summary>
/// The top-level scope of a <see cref="BakedInterpreter"/>.
/// </summary>
public class InterpreterContext : IBakedScope
{
    /// <inheritdoc />
    public IBakedScope? Parent { get; }

    /// <inheritdoc />
    public Dictionary<string, BakedObject> Variables { get; }

    internal InterpreterContext()
    {
        Parent = null;
        Variables = new Dictionary<string, BakedObject>();
    }
}
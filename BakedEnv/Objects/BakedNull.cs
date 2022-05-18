using BakedEnv.Interpreter;

namespace BakedEnv.Objects;

/// <summary>
/// Representation of null.
/// </summary>
public class BakedNull : BakedObject
{
    /// <inheritdoc />
    public override object? GetValue()
    {
        return null;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "null";
    }
}
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
    public override bool Equals(BakedObject? other)
    {
        return other is BakedNull;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return int.MinValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "null";
    }
}
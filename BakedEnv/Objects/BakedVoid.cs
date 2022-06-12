namespace BakedEnv.Objects;

/// <summary>
/// A type representing void.
/// </summary>
public class BakedVoid : BakedObject
{
    /// <inheritdoc />
    public override object? GetValue()
    {
        return null;
    }

    /// <inheritdoc />
    public override bool Equals(BakedObject? other)
    {
        return other is BakedVoid;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return int.MinValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "";
    }
}
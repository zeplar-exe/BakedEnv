namespace BakedEnv.Objects;

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
    public override string ToString()
    {
        return "void";
    }
}
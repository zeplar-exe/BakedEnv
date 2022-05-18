namespace BakedEnv.Objects;

public class BakedVoid : BakedObject
{
    public override object? GetValue()
    {
        return null;
    }

    public override bool Equals(object? obj)
    {
        return obj is BakedVoid;
    }

    public override bool Equals(BakedObject? other)
    {
        return other is BakedVoid;
    }

    public override string ToString()
    {
        return "void";
    }
}
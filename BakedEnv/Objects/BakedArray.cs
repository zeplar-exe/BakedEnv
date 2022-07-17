namespace BakedEnv.Objects;

public class BakedArray : BakedObject
{
    private BakedObject[] Values { get; }

    public BakedArray(IEnumerable<BakedObject> values)
    {
        Values = values.ToArray();
    }
    
    public override object? GetValue()
    {
        return Values;
    }

    public override int GetHashCode()
    {
        return Values.GetHashCode();
    }

    public override string ToString()
    {
        return string.Join<BakedObject>(", ", Values);
    }
}
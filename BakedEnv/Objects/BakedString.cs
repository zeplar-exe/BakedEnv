namespace BakedEnv.Objects;

/// <summary>
/// A string value.
/// </summary>
public class BakedString : BakedObject
{
    public string Value { get; }

    public BakedString(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public override object GetValue()
    {
        return Value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return Value.Equals(obj);
    }

    /// <inheritdoc />
    public override bool TryAdd(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;

        if (bakedObject is BakedString bakedString)
        {
            result = Add(bakedString);

            return true;
        }
        
        return false;
    }

    public BakedString Add(BakedString bakedString) => new(Value + bakedString.Value);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value;
    }
}
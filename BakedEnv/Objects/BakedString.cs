namespace BakedEnv.Objects;

/// <summary>
/// A string value.
/// </summary>
public class BakedString : BakedObject
{
    /// <summary>
    /// Raw value of this object.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initialize a BakedString with an initial value.
    /// </summary>
    /// <param name="value">Initial value.</param>
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

    /// <summary>
    /// Append <paramref name="other"/> to this string.
    /// </summary>
    /// <param name="other">String to append.</param>
    /// <returns>The resulting concatenated string.</returns>
    public BakedString Add(BakedString other) => new(Value + other.Value);

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
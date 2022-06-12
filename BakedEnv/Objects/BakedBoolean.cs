namespace BakedEnv.Objects;

/// <summary>
/// A boolean value.
/// </summary>
public class BakedBoolean : BakedObject
{
    /// <summary>
    /// The value of this boolean.
    /// </summary>
    public bool Value { get; }

    /// <summary>
    /// Initialize a BakedBoolean with an initial value.
    /// </summary>
    /// <param name="value">The initial value.</param>
    public BakedBoolean(bool value)
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
    public override bool TryNegate(out BakedObject? result)
    {
        result = Negate();

        return true;
    }

    /// <summary>
    /// Negate this boolean.
    /// </summary>
    /// <returns>Negated boolean object.</returns>
    public BakedBoolean Negate() => new(!Value);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString();
    }
}
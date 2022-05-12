using BakedEnv.Interpreter;

namespace BakedEnv.Objects;

/// <summary>
/// A boolean value.
/// </summary>
public class BakedBoolean : BakedObject
{
    public bool Value { get; }

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
    public override bool TryInvoke(IBakedScope scope, out BakedObject? returnValue)
    {
        returnValue = null;
        
        return false;
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

    public BakedBoolean Negate() => new(!Value);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <inheritdoc />
    public override bool TryGetContainedObject(string name, out BakedObject? bakedObject)
    {
        bakedObject = null;
        
        return false;
    }
    
    /// <inheritdoc />
    public override bool TrySetContainedObject(string name, BakedObject? bakedObject)
    {
        return false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString();
    }
}
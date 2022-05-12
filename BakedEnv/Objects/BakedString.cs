using BakedEnv.Interpreter;

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
    public override bool TryInvoke(BakedInterpreter interpreter, IBakedScope scope, out BakedObject? returnValue)
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
        return Value;
    }
}
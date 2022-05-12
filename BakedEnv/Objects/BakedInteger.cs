using System.Numerics;
using BakedEnv.Interpreter;

namespace BakedEnv.Objects;

/// <summary>
/// An integer value.
/// </summary>
/// <remarks>Uses a <see cref="BigInteger"/> implicitly.</remarks>
public class BakedInteger : BakedObject
{
    public BigInteger Value { get; }

    public BakedInteger()
    {
        Value = BigInteger.Zero;
    }
    
    public BakedInteger(BigInteger value)
    {
        Value = value;
    }

    public BakedInteger(ulong ulongValue)
    {
        Value = ulongValue;
    }
    
    public BakedInteger(long longValue)
    {
        Value = longValue;
    }
    
    public BakedInteger(decimal decimalValue)
    {
        Value = new BigInteger(decimalValue);
    }

    public BakedInteger(double doubleValue)
    {
        Value = new BigInteger(doubleValue);
    }
    
    public BakedInteger(float floatValue)
    {
        Value = new BigInteger(floatValue);
    }

    public BakedInteger(string stringValue)
    {
        if (BigInteger.TryParse(stringValue, out var value))
            Value = value;
        else if (decimal.TryParse(stringValue, out var decimalValue))
            Value = new BigInteger(decimalValue);
        else if (double.TryParse(stringValue, out var doubleValue))
            Value = new BigInteger(doubleValue);
        else if (float.TryParse(stringValue, out var floatValue))
            Value = new BigInteger(floatValue);
    }

    /// <inheritdoc />
    public override object GetValue()
    {
        return Value;
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
    public override bool TryInvoke(IBakedScope scope, out BakedObject? returnValue)
    {
        returnValue = null;
        
        return false;
    }

    /// <inheritdoc />
    /// <remarks>Only supports primitive numeric types and <see cref="BigInteger"/>.</remarks>
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            sbyte v => new BigInteger(v).Equals(Value),
            byte v => new BigInteger(v).Equals(Value),
            ushort v => new BigInteger(v).Equals(Value),
            short v => new BigInteger(v).Equals(Value),
            uint v => new BigInteger(v).Equals(Value),
            int v => new BigInteger(v).Equals(Value),
            ulong v => new BigInteger(v).Equals(Value),
            long v => new BigInteger(v).Equals(Value),
            float v => new BigInteger(v).Equals(Value),
            double v => new BigInteger(v).Equals(Value),
            decimal v => new BigInteger(v).Equals(Value),
            BigInteger v => v.Equals(Value),
            _ => false
        };
    }

    /// <inheritdoc />
    public override bool TryNegate(out BakedObject? result)
    {
        result = Negate();

        return true;
    }

    /// <inheritdoc />
    public BakedInteger Negate() => new(-Value);

    /// <inheritdoc />
    public override bool TryAdd(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Add(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Add(BakedInteger other) => new(Value + other.Value);

    /// <inheritdoc />
    public override bool TrySubtract(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Subtract(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Subtract(BakedInteger other) => new(Value - other.Value);

    /// <inheritdoc />
    public override bool TryMultiply(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Multiply(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Multiply(BakedInteger other) => new(Value * other.Value);

    /// <inheritdoc />
    public override bool TryExponent(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Exponent(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Exponent(BakedInteger other)
    {
        var integer = Value;

        for (var i = BigInteger.Zero; i < other.Value; i++)
        {
            integer *= other.Value;
        }

        return new BakedInteger(integer);
    }

    /// <inheritdoc />
    public override bool TryDivide(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Divide(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Divide(BakedInteger other) => new(Value / other.Value);

    /// <inheritdoc />
    public override bool TryModulus(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = Modulus(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public BakedInteger Modulus(BakedInteger other) => new(Value % other.Value);

    /// <inheritdoc />
    public override bool TryLessThan(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = LessThan(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool LessThan(BakedInteger other) => Value < other.Value;

    /// <inheritdoc />
    public override bool TryGreaterThan(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = GreaterThan(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool GreaterThan(BakedInteger other) => Value > other.Value;

    /// <inheritdoc />
    public override bool TryLessThanOrEqual(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = LessThanOrEqual(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool LessThanOrEqual(BakedInteger other) => Value <= other.Value;

    /// <inheritdoc />
    public override bool TryGreaterThanOrEqual(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedInteger bakedInteger)
        {
            result = GreaterThanOrEqual(bakedInteger);
            
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool GreaterThanOrEqual(BakedInteger other) => Value >= other.Value;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetValue().GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString();
    }
}
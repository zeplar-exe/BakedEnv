using System.Numerics;

namespace BakedEnv.Objects;

/// <summary>
/// An integer value.
/// </summary>
/// <remarks>Uses a <see cref="BigInteger"/> implicitly.</remarks>
public class BakedInteger : BakedObject
{
    /// <summary>
    /// BigInteger value of this object.
    /// </summary>
    public BigInteger Value { get; }

    /// <summary>
    /// Initialize a BakedInteger.
    /// </summary>
    public BakedInteger()
    {
        Value = BigInteger.Zero;
    }
    
    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="value">Initial value.</param>
    public BakedInteger(BigInteger value)
    {
        Value = value;
    }

    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="ulongValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.</remarks>
    public BakedInteger(ulong ulongValue)
    {
        Value = ulongValue;
    }
    
    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="longValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.</remarks>
    public BakedInteger(long longValue)
    {
        Value = longValue;
    }
    
    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="decimalValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedInteger(decimal decimalValue)
    {
        Value = new BigInteger(decimalValue);
    }

    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="doubleValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedInteger(double doubleValue)
    {
        Value = new BigInteger(doubleValue);
    }
    
    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="floatValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedInteger(float floatValue)
    {
        Value = new BigInteger(floatValue);
    }

    /// <summary>
    /// Initialize a BakedInteger with an initial value.
    /// </summary>
    /// <param name="stringValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// <br/>
    /// Conversion is attempted is the following order:
    /// <see cref="BigInteger"/> > <see cref="decimal"/> > <see cref="double"/> > <see cref="float"/><br/>
    /// If conversion is unsuccessful, the default value of 0 is used.</remarks>
    public BakedInteger(string stringValue)
    {
        if (BigInteger.TryParse(stringValue, out var value))
            Value = value;
        else if (decimal.TryParse(stringValue, out var decimalValue))
            Value = new BigInteger(decimalValue);
    }

    /// <inheritdoc />
    public override object GetValue()
    {
        return Value;
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
    
    /// <summary>
    /// Negate this integer.
    /// </summary>
    /// <returns>A negative version of this integer.</returns>
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
    
    /// <summary>
    /// Add <paramref name="other"/> to this integer.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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
    
    /// <summary>
    /// Subtract <paramref name="other"/> from this integer.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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

    /// <summary>
    /// Multiply this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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

    /// <summary>
    /// Raise this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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

    /// <summary>
    /// Divide this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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

    /// <summary>
    /// Apply a modulo operation in this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
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

    
    /// <summary>
    /// Determine if this integer is less than <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is less than <paramref name="other"/>.</returns>
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

    /// <summary>
    /// Determine if this integer is greater than <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is greater than <paramref name="other"/>.</returns>
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

    /// <summary>
    /// Determine if this integer is less than or equal to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is less than or equal to <paramref name="other"/>.</returns>
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

    /// <summary>
    /// Determine if this integer is greater than or equal to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is greater than or equal to <paramref name="other"/>.</returns>
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
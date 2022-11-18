using System.Globalization;
using System.Numerics;

namespace BakedEnv.Objects;

public class BakedDecimal : BakedObject
{
    /// <summary>
    /// BigInteger value of this object.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Initialize a BakedDecimal.
    /// </summary>
    public BakedDecimal()
    {
        Value = 0.0m;
    }
    
    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="decimalValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedDecimal(decimal decimalValue)
    {
        Value = decimalValue;
    }

    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="doubleValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedDecimal(double doubleValue)
    {
        Value = (decimal)doubleValue;
    }
    
    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="floatValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// Floating point types are truncated.</remarks>
    public BakedDecimal(float floatValue)
    {
        Value = (decimal)floatValue;
    }
    
    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="value">Initial value.</param>
    public BakedDecimal(BigInteger value)
    {
        Value = (decimal)value;
    }

    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="ulongValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.</remarks>
    public BakedDecimal(ulong ulongValue)
    {
        Value = ulongValue;
    }
    
    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="longValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.</remarks>
    public BakedDecimal(long longValue)
    {
        Value = longValue;
    }

    /// <summary>
    /// Initialize a BakedDecimal with an initial value.
    /// </summary>
    /// <param name="stringValue">Initial value.</param>
    /// <remarks>The initial value is converted/initialized as a BigInteger.
    /// <br/>
    /// Conversion is attempted is the following order:
    /// <see cref="BigInteger"/> > <see cref="decimal"/> > <see cref="double"/> > <see cref="float"/><br/>
    /// If conversion is unsuccessful, the default value of 0 is used.</remarks>
    public BakedDecimal(string stringValue)
    {
        if (BigInteger.TryParse(stringValue, out var value))
            Value = (decimal)value;
        else if (decimal.TryParse(stringValue, out var decimalValue))
            Value = decimalValue;
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
        return (decimal?)obj == Value;
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
    public BakedDecimal Negate() => new(-Value);

    /// <inheritdoc />
    public override bool TryAdd(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Add(bakedDecimal);
            
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Add <paramref name="other"/> to this integer.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Add(BakedDecimal other) => new(Value + other.Value);

    /// <inheritdoc />
    public override bool TrySubtract(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Subtract(bakedDecimal);
            
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Subtract <paramref name="other"/> from this integer.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Subtract(BakedDecimal other) => new(Value - other.Value);

    /// <inheritdoc />
    public override bool TryMultiply(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Multiply(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Multiply this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Multiply(BakedDecimal other) => new(Value * other.Value);

    /// <inheritdoc />
    public override bool TryExponent(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Exponent(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Raise this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Exponent(BakedDecimal other)
    {
        var integer = Value;

        for (var i = 0m; i < other.Value; i++)
        {
            integer *= other.Value;
        }

        return new BakedDecimal(integer);
    }

    /// <inheritdoc />
    public override bool TryDivide(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Divide(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Divide this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Divide(BakedDecimal other) => new(Value / other.Value);

    /// <inheritdoc />
    public override bool TryModulus(BakedObject bakedObject, out BakedObject? result)
    {
        result = null;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = Modulus(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Apply a modulo operation in this integer by <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>The resulting integer value.</returns>
    public BakedDecimal Modulus(BakedDecimal other) => new(Value % other.Value);

    /// <inheritdoc />
    public override bool TryLessThan(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = LessThan(bakedDecimal);
            
            return true;
        }

        return false;
    }

    
    /// <summary>
    /// Determine if this integer is less than <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is less than <paramref name="other"/>.</returns>
    public bool LessThan(BakedDecimal other) => Value < other.Value;

    /// <inheritdoc />
    public override bool TryGreaterThan(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = GreaterThan(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determine if this integer is greater than <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is greater than <paramref name="other"/>.</returns>
    public bool GreaterThan(BakedDecimal other) => Value > other.Value;

    /// <inheritdoc />
    public override bool TryLessThanOrEqual(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = LessThanOrEqual(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determine if this integer is less than or equal to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is less than or equal to <paramref name="other"/>.</returns>
    public bool LessThanOrEqual(BakedDecimal other) => Value <= other.Value;

    /// <inheritdoc />
    public override bool TryGreaterThanOrEqual(BakedObject bakedObject, out bool result)
    {
        result = false;
        
        if (bakedObject is BakedDecimal bakedDecimal)
        {
            result = GreaterThanOrEqual(bakedDecimal);
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determine if this integer is greater than or equal to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Right operand integer.</param>
    /// <returns>Whether this integer is greater than or equal to <paramref name="other"/>.</returns>
    public bool GreaterThanOrEqual(BakedDecimal other) => Value >= other.Value;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetValue().GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString(CultureInfo.CurrentCulture);
    }
}
namespace BakedEnv.Objects.Interfaces;

public interface IBakedInteger
{
    public BakedInteger Negate();
    public BakedInteger Add(BakedInteger other);
    public BakedInteger Subtract(BakedInteger other);
    public BakedInteger Multiply(BakedInteger other);
    public BakedInteger Exponent(BakedInteger other);
    public BakedInteger Divide(BakedInteger other);
    public BakedInteger Modulus(BakedInteger other);
    public bool LessThan(BakedInteger other);
    public bool GreaterThan(BakedInteger other);
    public bool LessThanOrEqual(BakedInteger other);
    public bool GreaterThanOrEqual(BakedInteger other);
}
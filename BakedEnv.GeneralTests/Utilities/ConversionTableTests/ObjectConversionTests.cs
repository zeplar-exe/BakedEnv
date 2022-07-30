using System.Numerics;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.Utilities.ConversionTableTests;

[TestFixture]
public class ObjectConversionTests
{
    [Test]
    public void TestSignedByteToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(sbyte.MaxValue));
    }
    
    [Test]
    public void TestByteToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(byte.MaxValue));
    }
    
    [Test]
    public void TestShortToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(short.MaxValue));
    }
    
    [Test]
    public void TestUnsignedShortToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(ushort.MaxValue));
    }
    
    [Test]
    public void TestIntToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(int.MaxValue));
    }
    
    [Test]
    public void TestUnsignedIntToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(uint.MaxValue));
    }
    
    [Test]
    public void TestLongToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(long.MaxValue));
    }
    
    [Test]
    public void TestUnsignedLongToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(ulong.MaxValue));
    }
    
    [Test]
    public void TestFloatToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(float.MaxValue));
    }
    
    [Test]
    public void TestDoubleToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(double.MaxValue));
    }
    
    [Test]
    public void TestDecimalToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(decimal.MaxValue));
    }
    
    [Test]
    public void TestBigIntegerToInteger()
    {
        Assert.True(ConvertedValueIs<BakedInteger>(BigInteger.One));
    }

    [Test]
    public void TestInvalidConversion()
    {
        Assert.False(ConvertedValueIs<BakedString>(0));
    }

    private bool ConvertedValueIs<T>(object value) where T : BakedObject
    {
        var table = MappedConversionTable.Primitive();
        var success = table.TryToBakedObject(value, out var bakedObject);

        return success && bakedObject is T && bakedObject.Equals(value);
    }
}
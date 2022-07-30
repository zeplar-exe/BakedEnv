using System;
using System.Numerics;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.Utilities.ConversionTableTests;

public class BakedObjectConversionTests
{
    [Test]
    public void TestIntegerToSignedByte()
    {
        Assert.True(GetValueNotNull(new BakedInteger(1), typeof(sbyte))
            .Equals((sbyte)0b1));
    }
    
    [Test]
    public void TestIntegerToByte()
    {
        Assert.True(GetValueNotNull(new BakedInteger(1), typeof(byte))
            .Equals((byte)0b1));
    }
    
    [Test]
    public void TestIntegerToShort()
    {
        Assert.True(GetValueNotNull(new BakedInteger(200), typeof(short))
            .Equals((short)200));
    }
    
    [Test]
    public void TestIntegerToUnsignedShort()
    {
        Assert.True(GetValueNotNull(new BakedInteger(200), typeof(ushort))
            .Equals((ushort)200));
    }
    
    [Test]
    public void TestIntegerToInt()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(int))
            .Equals(500));
    }
    
    [Test]
    public void TestIntegerToUnsignedInt()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(uint))
            .Equals(500U));
    }
    
    [Test]
    public void TestIntegerToLong()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(long))
            .Equals(500L));
    }

    [Test]
    public void TestIntegerToUnsignedLong()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(ulong))
            .Equals(500UL));
    }
    
    [Test]
    public void TestIntegerToFloat()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(float))
            .Equals(500F));
    }
    
    [Test]
    public void TestIntegerToDouble()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(double))
            .Equals(500D));
    }
    
    [Test]
    public void TestIntegerToDecimal()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(decimal))
            .Equals(500M));
    }
    
    [Test]
    public void TestIntegerToBigInteger()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(BigInteger))
            .Equals(new BigInteger(500)));
    }

    [Test]
    public void TestStringToString()
    {
        Assert.True(GetValueNotNull(new BakedString("Foo"), typeof(string))
            .Equals("Foo"));
    }

    [Test]
    public void TestBooleanToBool()
    {
        Assert.True(GetValueNotNull(new BakedBoolean(true), typeof(bool))
            .Equals(true));
    }

    [Test]
    public void TestInvalidConversion()
    {
        Assert.Null(GetValue(new BakedInteger(1), typeof(string)));
    }
    
    private object GetValueNotNull(BakedObject bakedObject, Type target)
    {
        var value = GetValue(bakedObject, target);
        
        if (value == null)
            Assert.Fail();

        return value!;
    }

    private object? GetValue(BakedObject bakedObject, Type target)
    {
        return MappedConversionTable.Primitive().TryToObject(bakedObject, target, out var o) ? o : null;
    }
}
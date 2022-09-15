using System;
using System.Numerics;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.Utilities.ConversionTableTests;

public class BakedObjectConversionTests
{
    [TestCase(1, typeof(sbyte))]
    [TestCase(1, typeof(byte))]
    [TestCase(200, typeof(short))]
    [TestCase(200, typeof(ushort))]
    [TestCase(500, typeof(int))]
    [TestCase(500, typeof(uint))]
    [TestCase(500, typeof(long))]
    [TestCase(500, typeof(ulong))]
    public void TestInteger(int integer, Type targetType)
    {
        var converted = Convert.ChangeType(integer, targetType);
        
        Assert.True(GetValueNotNull(new BakedInteger(integer), targetType).Equals(converted));
    }
    
    [Test]
    public void TestIntegerToBigInteger()
    {
        Assert.True(GetValueNotNull(new BakedInteger(500), typeof(BigInteger))
            .Equals(new BigInteger(500)));
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
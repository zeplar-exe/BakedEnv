using System;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.Utilities.ConversionTableTests;

[TestFixture]
public class OverflowTests
{
    [Test]
    public void TestIntegerToSignedByteOverflow()
    {
        Assert.True(GetValueNotNull(new BakedInteger(sbyte.MaxValue + 1), typeof(sbyte))
            .Equals(sbyte.MinValue));
    }
    
    [Test]
    public void TestIntegerToByteOverflow()
    {
        Assert.True(GetValueNotNull(new BakedInteger(byte.MaxValue + 1), typeof(byte))
            .Equals(byte.MinValue));
    }
    
    [Test]
    public void TestIntegerToShortOverflow()
    {
        Assert.True(GetValueNotNull(new BakedInteger(short.MaxValue + 1), typeof(short))
            .Equals(short.MinValue));
    }
    
    [Test]
    public void TestIntegerToUnsignedShortOverflow()
    {
        Assert.True(GetValueNotNull(new BakedInteger(ushort.MaxValue + 1), typeof(ushort))
            .Equals(ushort.MinValue));
    }
    
    private object GetValueNotNull(BakedObject bakedObject, Type target)
    {
        var value = GetValue(bakedObject, target);
        
        if (value == null)
            Assert.Fail();

        return new MappedConversionTable().ToObject(bakedObject, target)!;
    }

    private object? GetValue(BakedObject bakedObject, Type target)
    {
        return new MappedConversionTable().ToObject(bakedObject, target);
    }
}
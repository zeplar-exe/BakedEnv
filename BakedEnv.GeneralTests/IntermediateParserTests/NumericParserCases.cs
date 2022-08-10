using System;
using System.Globalization;

using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

public class NumericParserCases
{
    [Test]
    public void NumericTokenIsValidParse()
    {
        var token = ParserHelper.AssertFirstIs<NumericToken>("123.456");

        var (digitsConcat, mantissaConcat) = ReadNumericToken(token);
        
        Assert.That(double.TryParse(digitsConcat, out _), Is.True);
        Assert.That(double.TryParse(mantissaConcat, out _), Is.True);
    }

    [Test]
    public void ZeroIsValid()
    {
        AssertNumericTokenIsEqual(0);
    }

    [Test]
    public void IntegerIsValid()
    {
        AssertNumericTokenIsEqual(123);
    }

    [Test]
    public void DecimalIsValid()
    {
        AssertNumericTokenIsEqual(123, 456);
        AssertNumericTokenIsEqual(789, 10);
        AssertNumericTokenIsEqual(0, 987);
    }

    [Test]
    public void TrailingDecimalPointIsInvalid()
    {
        var token = ParserHelper.AssertFirstIs<NumericToken>("123.");
        
        Assert.That(token.IsComplete, Is.False);
    }

    private void AssertNumericTokenIsEqual(int digits, int? mantissa = null)
    {
        var n = BuildDouble(digits, mantissa).ToString(CultureInfo.InvariantCulture);
        var token = ParserHelper.AssertFirstIs<NumericToken>(n);

        var (digitsConcat, mantissaConcat) = ReadNumericToken(token);
        
        Assert.That(digitsConcat, Is.EqualTo(digitsConcat));
        Assert.That(mantissaConcat, Is.EqualTo(mantissaConcat));
    }

    private double BuildDouble(int digits, int? mantissa)
    {
        var result = (double)digits;

        if (mantissa == null)
            return result;

        var digitCount = (int)Math.Floor(Math.Log10(mantissa.Value) + 1);
        var power = Math.Pow(10, digitCount);
        
        result += mantissa.Value / power;

        return result;
    }

    private (string, string) ReadNumericToken(NumericToken token)
    {
        var digitsConcat = string.Concat(token.Digits);
        var mantissaConcat = string.Concat(token.Digits);

        return (digitsConcat, mantissaConcat);
    }
}
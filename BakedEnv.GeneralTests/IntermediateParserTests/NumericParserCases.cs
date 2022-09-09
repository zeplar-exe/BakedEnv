using System;
using System.Globalization;
using System.Linq;

using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

public class NumericParserCases
{
    [Test]
    public void NumericTokenIsValidParse()
    {
        var token = ParserHelper.AssertFirstIs<DecimalToken>("123.456");

        var (digitsConcat, mantissaConcat) = ReadDecimalToken(token);
        
        Assert.That(double.TryParse(digitsConcat, out _), Is.True);
        Assert.That(double.TryParse(mantissaConcat, out _), Is.True);
        Assert.That(digitsConcat, Is.EqualTo("123"));
        Assert.That(mantissaConcat, Is.EqualTo("456"));
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

    // Word of warning: doubles do not retain trailing zeros.
    // You could use string formatting hacks to imitate this,
    // however, it's very volatile in future terms and will
    // probably break stuff
    [TestCase(123, 456)]
    [TestCase(789, 1)]
    [TestCase(0, 987)]
    public void DecimalIsValid(int digits, int mantissa)
    {
        AssertNumericTokenIsEqual(digits, mantissa);
    }

    [Test]
    public void TrailingDecimalPointIsInvalid()
    {
        var token = ParserHelper.AssertFirstIs<DecimalToken>("123.");
        
        Assert.That(token.IsComplete, Is.False);
    }

    private void AssertNumericTokenIsEqual(int digits, int? mantissa = null)
    {
        var n = BuildDouble(digits, mantissa).ToString(CultureInfo.InvariantCulture);
        Assert.That(ParserHelper.TryGetFirst(n, out var token), Is.True);

        if (token is DecimalToken decimalToken)
        {
            var (digitsConcat, mantissaConcat) = ReadDecimalToken(decimalToken);
            
            Assert.That(digitsConcat, Is.EqualTo(digits.ToString()));
            Assert.That(mantissaConcat, Is.EqualTo(mantissa.ToString()));
        }
        else if (token is IntegerToken integerToken)
        {
            var integerConcat = ReadIntegerToken(integerToken);
            
            Assert.That(integerConcat, Is.EqualTo(digits.ToString()));
        }
        else
        {
            Assert.Fail($"Expected decimal or integer token, got '{token?.GetType().Name ?? "null"}'");
        }
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

    private string ReadIntegerToken(IntegerToken token)
    {
        var digitsConcat = string.Concat(token.Digits);

        return digitsConcat;
    }
    
    private (string, string) ReadDecimalToken(DecimalToken token)
    {
        var digitsConcat = string.Concat(token.Digits);
        var mantissaConcat = string.Concat(token.Mantissa);

        return (digitsConcat, mantissaConcat);
    }
}
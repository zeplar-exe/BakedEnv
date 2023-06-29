using System.Numerics;

using BakedEnv.GeneralTests.IntermediateParserTests;
using BakedEnv.Interpreter.IntermediateTokens;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateFunctionality.Parsers;

[TestFixture]
public class IntegerParseCases
{
    [TestCase("0")]
    [TestCase("123456")]
    [TestCase("02")]
    [TestCase("90000")]
    [TestCase("7922816251950335")]
    [TestCase("3375935439503354")]
    public void AnyDecimalIsValid(string d)
    {
        AssertIntegerTokenContentIsEqualTo(d);
    }

    private void AssertIntegerTokenContentIsEqualTo(string d)
    {
        var inputInteger = BigInteger.Parse(d);
        var token = ParserHelper.AssertFirstIs<IntegerToken>(d);

        var parsedDecimal = BigInteger.Parse(token.ToString());
        
        Assert.That(parsedDecimal, Is.EqualTo(inputInteger));
    }
}
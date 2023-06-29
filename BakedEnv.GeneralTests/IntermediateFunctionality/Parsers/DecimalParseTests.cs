using BakedEnv.GeneralTests.IntermediateParserTests;
using BakedEnv.Interpreter.IntermediateTokens;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateFunctionality.Parsers;

[TestFixture]
public class DecimalParseTests
{
    [TestCase("0.1")]
    [TestCase("123.456")]
    [TestCase("0.0002")]
    [TestCase("2.0093")]
    [TestCase("9.0000")]
    [TestCase("7922816251950335.0")]
    [TestCase("337593543950335.0")]
    public void AnyDecimalIsValid(string d)
    {
        AssertDecimalTokenContentIsEqualTo(d);
    }

    private void AssertDecimalTokenContentIsEqualTo(string d)
    {
        var inputDecimal = decimal.Parse(d);
        var token = ParserHelper.AssertFirstIs<DecimalToken>(d);
        
        var parsedDecimal = decimal.Parse(token.ToString());
        
        Assert.That(parsedDecimal, Is.EqualTo(inputDecimal));
    }
}
using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.LexerTests.Text;

[TestFixture]
public class NumericTests
{
    private static string TestString => "0123456789";
    
    [Test]
    public void TestDigits()
    {
        var tokens = TokenHelper.GetTokens(TestString);
        
        TokenHelper.AssertTokensLength(tokens, 1);
        TokenHelper.AssertValidToken(tokens[0], TestString, TextualTokenType.Numeric);
    }

    [Test]
    public void TestCombinedDigits()
    {
        var combinedString = $"{TestString}.{TestString}f";
        var tokens = TokenHelper.GetTokens(combinedString);
        
        TokenHelper.AssertTokensLength(tokens, 4);
        
        TokenHelper.AssertValidToken(tokens[0], TestString, TextualTokenType.Numeric);
        TokenHelper.AssertValidToken(tokens[1], '.', TextualTokenType.Period);
        TokenHelper.AssertValidToken(tokens[2], TestString, TextualTokenType.Numeric);
        TokenHelper.AssertValidToken(tokens[3], 'f', TextualTokenType.Alphabetic);
    }
}
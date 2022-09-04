using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.LexerTests.Text;

[TestFixture]
public class AlphabeticTests
{
    private static string TestString => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [Test]
    public void TestFullString()
    {
        var tokens = TokenHelper.GetTokens(TestString);
        
        TokenHelper.AssertTokensLength(tokens, 1);
        TokenHelper.AssertValidToken(tokens[0], TestString, TextualTokenType.Alphabetic);
    }

    [Test]
    public void TestStringCombination()
    {
        var combinedString = $"{TestString}|123|{TestString}";
        var tokens = TokenHelper.GetTokens(combinedString);
        
        TokenHelper.AssertTokensLength(tokens, 5);
        
        TokenHelper.AssertValidToken(tokens[0], TestString, TextualTokenType.Alphabetic);
        TokenHelper.AssertValidToken(tokens[1], '|', TextualTokenType.Vertical);
        TokenHelper.AssertValidToken(tokens[2], "123", TextualTokenType.Numeric);
        TokenHelper.AssertValidToken(tokens[3],'|', TextualTokenType.Vertical);
        TokenHelper.AssertValidToken(tokens[4], TestString, TextualTokenType.Alphabetic);
    }
}
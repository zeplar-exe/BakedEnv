using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.LexerTests.Text;

[TestFixture]
public class WhitespaceTests
{
    private static object[] Cases =
    {
        new object[] { " ", TextualTokenType.Space },
        new object[] { "\t", TextualTokenType.Tab },
        new object[] { "\r", TextualTokenType.NewLine },
        new object[] { "\n", TextualTokenType.NewLine },
        new object[] { "\r\n", TextualTokenType.NewLine }
    };
    
    [TestCaseSource(nameof(Cases))]
    public void TestSingleWhitespace(string text, TextualTokenType type)
    {
        var tokens = TokenHelper.GetTokens(text);
        
        TokenHelper.AssertTokensLength(tokens, 1);
        TokenHelper.AssertValidToken(tokens[0], text, type);
    }
    
    [TestCaseSource(nameof(Cases))]
    public void TestCombinedWhitespace(string text, TextualTokenType type)
    {
        var tokens = TokenHelper.GetTokens($"{text}abc{text}");
        
        TokenHelper.AssertTokensLength(tokens, 3);
        
        TokenHelper.AssertValidToken(tokens[0], text, type);
        TokenHelper.AssertValidToken(tokens[1], "abc", TextualTokenType.Alphabetic);
        TokenHelper.AssertValidToken(tokens[2], text, type);
    }
}
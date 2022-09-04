using System.Linq;

using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.LexerTests.Text;

[TestFixture]
public class SymbolTests
{
    private static object[] Cases =
    {
        new object[] { '~', TextualTokenType.Tilde },
        new object[] {'~', TextualTokenType.Tilde },
        new object[] {'`', TextualTokenType.Backtick },
        new object[] {'!', TextualTokenType.ExclamationMark },
        new object[] {'@', TextualTokenType.At },
        new object[] {'#', TextualTokenType.Hashtag },
        new object[] {'$', TextualTokenType.Dollar },
        new object[] {'%', TextualTokenType.Percent },
        new object[] {'^', TextualTokenType.Caret },
        new object[] {'&', TextualTokenType.Ampersand },
        new object[] {'*', TextualTokenType.Star },
        new object[] {'(', TextualTokenType.LeftParenthesis },
        new object[] {')', TextualTokenType.RightParenthesis },
        new object[] {'_', TextualTokenType.Underscore },
        new object[] {'-', TextualTokenType.Dash },
        new object[] {'+', TextualTokenType.Plus },
        new object[] {'=', TextualTokenType.Equals },
        new object[] {'{', TextualTokenType.LeftCurlyBracket },
        new object[] {'[', TextualTokenType.LeftBracket },
        new object[] {'}', TextualTokenType.RightCurlyBracket },
        new object[] {']', TextualTokenType.RightBracket },
        new object[] {'|', TextualTokenType.Vertical },
        new object[] {'\\', TextualTokenType.Backslash },
        new object[] {':', TextualTokenType.Colon },
        new object[] {';', TextualTokenType.Semicolon },
        new object[] {'\"', TextualTokenType.DoubleQuotation },
        new object[] {'\'', TextualTokenType.SingleQuotation },
        new object[] {'<', TextualTokenType.LessThan },
        new object[] {',', TextualTokenType.Comma },
        new object[] {'>', TextualTokenType.GreaterThan },
        new object[] {'.', TextualTokenType.Period },
        new object[] {'?', TextualTokenType.QuestionMark },
        new object[] {'/', TextualTokenType.Slash },
    };
    
    [TestCaseSource(nameof(Cases))]
    public void TestClassification(char character, TextualTokenType type)
    {
        var tokens = TokenHelper.GetTokens(character.ToString());
        
        TokenHelper.AssertTokensLength(tokens, 1);

        var token = tokens.First();
        
        Assert.AreEqual(token.Type, type);
    }

    [TestCaseSource(nameof(Cases))]
    public void TestCombination(char character, TextualTokenType type)
    {
        var testString = $"{character}:{character}";
        var tokens = TokenHelper.GetTokens(testString);
        
        TokenHelper.AssertTokensLength(tokens, 3);
        
        TokenHelper.AssertValidToken(tokens[0], character, type);
        TokenHelper.AssertValidToken(tokens[1], ':', TextualTokenType.Colon);
        TokenHelper.AssertValidToken(tokens[2], character, type);
    }
}
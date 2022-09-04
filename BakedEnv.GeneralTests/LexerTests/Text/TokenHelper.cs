using System.Linq;

using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.LexerTests.Text;

public static class TokenHelper
{
    public static void AssertValidToken(TextualToken token, char character, TextualTokenType type)
    {
        AssertValidToken(token, character.ToString(), type);
    }
    
    public static void AssertValidToken(TextualToken token, string text, TextualTokenType type)
    {
        Assert.That(token.Text, Is.EqualTo(text));
        Assert.That(token.Type, Is.EqualTo(type));
        Assert.That(token.Length, Is.EqualTo(text.Length));
    }

    public static void AssertTokensLength(TextualToken[] tokens, int length)
    {
        Assert.That(tokens, Has.Length.EqualTo(length), $"Expected TextLexer enumeration length of {length}.");
    }
    
    public static TextualToken[] GetTokens(string input)
    {
        var lexer = new TextLexer(input);
        var tokens = lexer.ToArray();

        Assert.That(tokens, Is.Not.Empty, "TextLexer enumeration returned empty enumeration.");

        return tokens;
    }
}
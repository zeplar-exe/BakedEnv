using System.Diagnostics.CodeAnalysis;
using System.Linq;

using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.Lexer;

using NUnit.Framework;



namespace BakedEnv.GeneralTests.IntermediateParserTests;

internal static class ParserHelper
{
    public static ParserIterator CreateIterator(string text)
    {
        var lexer = new TextLexer(text);
        var iterator = new ParserIterator(lexer);

        return iterator;
    }

    public static bool TryGetFirst(string text, [NotNullWhen(true)] out IntermediateToken? token)
    {
        var root = RootParser.Default();

        token = root.Parse(CreateIterator(text)).FirstOrDefault();

        return token != null && token is not EndOfFileToken;
    }

    public static T AssertFirstIs<T>(string input) where T : IntermediateToken
    {
        Assert.That(TryGetFirst(input, out var token), Is.True);
        Assert.That(token, Is.TypeOf<T>());

        return (T)token!;
    }
}
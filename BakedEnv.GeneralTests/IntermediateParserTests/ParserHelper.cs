using System.Diagnostics.CodeAnalysis;
using System.Linq;

using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

internal static class ParserHelper
{
    public static ParserIterator CreateIterator(string text)
    {
        var lexer = new Lexer(text);
        var iterator = new ParserIterator(lexer);

        return iterator;
    }

    public static bool TryGetFirst(string text, [NotNullWhen(true)] out IntermediateToken? token)
    {
        var root = RootParser.Default();

        token = root.Parse(CreateIterator(text)).FirstOrDefault();

        return token != null && token is not EndOfFileToken;
    }
}
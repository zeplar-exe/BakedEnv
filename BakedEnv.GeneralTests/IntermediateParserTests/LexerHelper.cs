using BakedEnv.Common;

using TokenCs;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

internal static class LexerHelper
{
    public static EnumerableIterator<LexerToken> CreateIterator(string text)
    {
        var lexer = new Lexer(text);
        var iterator = new EnumerableIterator<LexerToken>(lexer);

        return iterator;
    }
}
using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class ProcessorStatementParser
{
    public ProcessorStatementToken Parse(LeftBracketToken leftBracket, ParserIterator iterator)
    {
        var token = new ProcessorStatementToken
        {
            LeftBracket = leftBracket
        };

        if (!iterator.NextIs(LexerTokenType.RightBracket, out var rightBracket))
            return token.AsIncomplete();

        token.RightBracket = new RightBracketToken(rightBracket);

        return token;
    }
}
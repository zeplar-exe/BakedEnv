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

        while (iterator.SkipTrivia(out var next))
        {
            switch (next.Type)
            {
                case LexerTokenType.RightBracket:
                {
                    token.RightBracket = new RightBracketToken(next);

                    return token.AsComplete();
                }
            }
        }

        return token.AsIncomplete();
    }
}
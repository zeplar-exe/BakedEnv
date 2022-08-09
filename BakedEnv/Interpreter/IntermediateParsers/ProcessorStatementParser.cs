using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class ProcessorStatementParser : MatchParser
{
    public override TryMatchResult TryParse(LexerToken first, ParserIterator iterator)
    {
        if (!TestTokenIs(first, LexerTokenType.LeftBracket))
            return TryMatchResult.NotMatch();
        
        var token = new ProcessorStatementToken
        {
            LeftBracket = new LeftBracketToken(first)
        };

        while (iterator.SkipTrivia(out var next))
        {
            switch (next.Type)
            {
                case LexerTokenType.RightBracket:
                {
                    token.RightBracket = new RightBracketToken(next);

                    return new TryMatchResult(true, token.AsComplete());
                }
            }
        }
        
        return TryMatchResult.MatchSuccess(token.AsIncomplete());
    }
}
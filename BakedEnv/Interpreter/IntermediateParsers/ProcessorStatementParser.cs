using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class ProcessorStatementParser : MatchParser
{
    public override bool Match(LexerToken first)
    {
        return TestTokenIs(first, LexerTokenType.LeftBracket);
    }

    public override IntermediateToken Parse(LexerToken first, ParserIterator iterator)
    {
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

                    return token.AsComplete();
                }
                default:
                {
                    iterator.Reserve();

                    return token.AsIncomplete();
                }
            }
        }
        
        return token.AsIncomplete();
    }
}
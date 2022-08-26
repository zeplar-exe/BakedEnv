using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateParsers;

internal class ProcessorStatementParser : MatchParser
{
    public override bool Match(TextualToken first)
    {
        return TestTokenIs(first, TextualTokenType.LeftBracket);
    }

    public override IntermediateToken Parse(TextualToken first, ParserIterator iterator)
    {
        var token = new ProcessorStatementToken
        {
            LeftBracket = new LeftBracketToken(first)
        };

        while (iterator.SkipTrivia(out var next))
        {
            switch (next.Type)
            {
                case TextualTokenType.RightBracket:
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
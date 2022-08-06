using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class StringParser : MatchParser
{
    public override TryMatchResult TryParse(LexerToken first, ParserIterator iterator)
    {
        if (!TestTokenIs(first, LexerTokenType.SingleQuotation, LexerTokenType.DoubleQuotation))
            return TryMatchResult.NotMatch();
        
        var token = new StringToken
        {
            LeftQuotation = new QuotationToken(first)
        };

        LexerToken? escapeToken = null;

        while (iterator.TryMoveNext(out var next))
        {
            if (escapeToken == null)
            {
                if (next.Type == first.Type)
                {
                    token.RightQuotation = new QuotationToken(next);

                    return TryMatchResult.MatchSuccess(token.AsComplete());
                }
            }
            else
            {
                var escapeContent = new StringContentToken(escapeToken);
            
                token.Content.Add(escapeContent);
            }

            escapeToken = null;
            
            var content = new StringContentToken(next);
            
            if (next.Type == LexerTokenType.Backslash)
            {
                escapeToken = next;
            }
            else
            {
                token.Content.Add(content);
            }
        }

        return TryMatchResult.MatchSuccess(token.AsIncomplete());
    }
}
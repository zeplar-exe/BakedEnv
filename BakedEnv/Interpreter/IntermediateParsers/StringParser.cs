using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class StringParser : MatchParser
{
    public override bool Match(LexerToken first)
    {
        return TestTokenIs(first, LexerTokenType.SingleQuotation, LexerTokenType.DoubleQuotation);
    }

    public override IntermediateToken Parse(LexerToken first, ParserIterator iterator)
    {
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

                    return token.AsComplete();
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

        return token.AsIncomplete();
    }
}
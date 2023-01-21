using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateParsers;

public class StringIntermediateParser : MatchIntermediateParser
{
    public override bool Match(TextualToken first)
    {
        return TestTokenIs(first, TextualTokenType.SingleQuotation, TextualTokenType.DoubleQuotation);
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        var token = new StringToken
        {
            LeftQuotation = new QuotationToken(first)
        };

        TextualToken? escapeToken = null;

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
                var escapeContent = new AnyToken(escapeToken);
            
                token.Content.Add(escapeContent);
            }

            escapeToken = null;
            
            var content = new AnyToken(next);
            
            if (next.Type == TextualTokenType.Backslash)
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
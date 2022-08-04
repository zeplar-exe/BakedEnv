using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class StringParser
{
    public StringToken Parse(QuotationToken quotation, ParserIterator iterator)
    {
        var token = new StringToken { LeftQuotation = quotation };

        LexerToken? escapeToken = null;

        while (iterator.TryMoveNext(out var next))
        {
            if (escapeToken == null)
            {
                if (next.Type == quotation.Type)
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
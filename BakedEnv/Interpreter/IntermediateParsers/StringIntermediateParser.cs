using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
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
            Open = new RawIntermediateToken(first)
        };

        TextualToken? escapeToken = null;

        while (iterator.TryMoveNext(out var next))
        {
            if (escapeToken == null)
            {
                if (next.Type == first.Type)
                {
                    token.Close = new RawIntermediateToken(next);

                    return token.AsComplete();
                }
            }
            else
            {
                var escapeContent = new RawIntermediateToken(escapeToken);
            
                token.Content.Add(escapeContent);
            }

            escapeToken = null;
            
            var content = new RawIntermediateToken(next);
            
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
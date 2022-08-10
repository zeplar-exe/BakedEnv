using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class NumericParser : MatchParser
{
    public override bool Match(LexerToken first)
    {
        return TestTokenIs(first, LexerTokenType.Numeric);
    }

    public override IntermediateToken Parse(LexerToken first, ParserIterator iterator)
    {
        var token = new NumericToken
        {
            Digits = { new DigitsToken(first) }
        };

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
            {
                if (token.DecimalPoint == null)
                    return token.AsComplete();
                
                return token.AsIncomplete();
            }

            switch (next.Type)
            {
                case LexerTokenType.Numeric:
                    var digit = new DigitsToken(next);
                    
                    if (token.DecimalPoint == null)
                        token.Digits.Add(digit);
                    else
                        token.Mantissa.Add(digit);
                    
                    break;
                case LexerTokenType.Period:
                    token.DecimalPoint = new PeriodToken(next);
                    
                    break;
                default:
                    iterator.Reserve();

                    return token.AsComplete();
            }
        }
    }
}
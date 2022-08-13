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

        while (iterator.TryMoveNext(out var next))
        {
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

                    if (token.DecimalPoint != null && token.Mantissa.Count == 0)
                        return token.AsIncomplete();
                    
                    return token.AsComplete();
            }
        }

        if (token.Digits.Count == 0) // Great example of an impossible condition to be safe
            return token.AsIncomplete();
        
        if (token.DecimalPoint != null && token.Mantissa.Count == 0)
            return token.AsIncomplete();
                    
        return token.AsComplete();
    }
}
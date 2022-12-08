using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;

using IntegerToken = BakedEnv.Interpreter.IntermediateTokens.Pure.IntegerToken;


namespace BakedEnv.Interpreter.IntermediateParsers;

public class NumericIntermediateParser : MatchIntermediateParser
{
    public override bool Match(TextualToken first)
    {
        return TestTokenIs(first, TextualTokenType.Numeric);
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        var token = new IntegerToken
        {
            Digits = { new DigitsToken(first) }
        };

        while (iterator.TryMoveNext(out var next))
        {
            switch (next.Type)
            {
                case TextualTokenType.Numeric:
                    var digit = new DigitsToken(next);
                    
                    token.Digits.Add(digit);
                    break;
                case TextualTokenType.Period:
                    var period = new PeriodToken(next);
                    
                    return ParseDecimalToken(iterator, token, period);
                default:
                    iterator.Reserve();
                    
                    return token.AsComplete();
            }
        }
                    
        return token.AsComplete();
    }

    private DecimalToken ParseDecimalToken(LexerIterator iterator, IntegerToken integerToken, PeriodToken period)
    {
        var decimalToken = new DecimalToken { DecimalPoint = period };
        decimalToken.Digits.AddRange(integerToken.Digits);

        while (iterator.TryMoveNext(out var next))
        {
            switch (next.Type)
            {
                case TextualTokenType.Numeric:
                    var digit = new DigitsToken(next);
                    
                    decimalToken.Mantissa.Add(digit);
                    
                    break;
                default:
                    iterator.Reserve();

                    if (decimalToken.Mantissa.Count == 0)
                        return decimalToken.AsIncomplete();
                    
                    return decimalToken.AsComplete();
            }
        }
        
        if (decimalToken.Mantissa.Count == 0)
            return decimalToken.AsIncomplete();
        
        return decimalToken.AsComplete();
    }
}
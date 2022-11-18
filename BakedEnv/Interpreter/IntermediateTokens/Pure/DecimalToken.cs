using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens.Interfaces;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class DecimalToken : PureIntermediateToken, IExpressionToken
{
    public List<DigitsToken> Digits { get; }
    public PeriodToken? DecimalPoint { get; set; }
    public List<DigitsToken> Mantissa { get; }

    public DecimalToken()
    {
        Digits = new List<DigitsToken>();
        Mantissa = new List<DigitsToken>();
    }
    
    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            foreach (var digit in Digits)
            {
                yield return digit;
            }

            if (DecimalPoint != null) yield return DecimalPoint;

            foreach (var digit in Mantissa)
            {
                yield return digit;
            }
        }
    }

    public BakedExpression CreateExpression()
    {
        AssertComplete(); // Note that completeness ensures a valid numeric token,
        // thus ensuring that double.Parse will always be successful

        var digits = string.Concat(Digits);
        var mantissa = "0";
        
        if (Mantissa.Count > 0)
            mantissa = string.Concat(Mantissa);

        var digitsValue = double.Parse(digits);
        var mantissaValue = double.Parse(mantissa);
        var shiftedMantissa = mantissaValue / (10.0 * Digits.Count);

        var value = digitsValue + shiftedMantissa;
        var d = new BakedDecimal(value);

        return new ValueExpression(d);
    }
}
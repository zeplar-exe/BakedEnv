using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class DecimalToken : PureIntermediateToken
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
}
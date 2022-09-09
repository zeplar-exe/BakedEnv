using System.Numerics;

using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens.Interfaces;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class IntegerToken : PureIntermediateToken, IExpressionToken
{
    public List<DigitsToken> Digits { get; }

    public IntegerToken()
    {
        Digits = new List<DigitsToken>();
    }
    
    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            foreach (var digit in Digits)
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
        var digitsValue = BigInteger.Parse(digits);

        var i = new BakedInteger(digitsValue);

        return new ValueExpression(i);
    }
}
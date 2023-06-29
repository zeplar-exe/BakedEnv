using System.Numerics;

using BakedEnv.Interpreter.Expressions;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class IntegerToken : IntermediateToken
{
    public List<ILowLevelToken> Digits { get; }

    public override long StartIndex => Digits.First().StartIndex;
    public override long Length => Digits.Sum(t => t.Length);
    public override long EndIndex => Digits.Last().EndIndex;

    public IntegerToken()
    {
        Digits = new List<ILowLevelToken>();
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
    
    public override string ToString()
    {
        return string.Concat(Digits);
    }
}
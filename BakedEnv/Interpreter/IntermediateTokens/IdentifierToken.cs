using System.Numerics;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class IdentifierToken : IntermediateToken
{
    public List<ILowLevelToken> Tokens { get; }

    public override long StartIndex => Tokens.First().StartIndex;
    public override long Length => Tokens.Sum(t => t.Length);
    public override long EndIndex => Tokens.Last().EndIndex;

    public IdentifierToken()
    {
        Tokens = new List<ILowLevelToken>();
    }
    
    public BigInteger AsBigInteger() => BigInteger.Parse(ToString());
    
    public override string ToString()
    {
        return string.Concat(Tokens);
    }
}
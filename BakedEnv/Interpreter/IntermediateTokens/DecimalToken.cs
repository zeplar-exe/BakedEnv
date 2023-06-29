namespace BakedEnv.Interpreter.IntermediateTokens;

public class DecimalToken : IntermediateToken
{
    public List<ILowLevelToken> Digits { get; }
    public ILowLevelToken? DecimalPoint { get; set; }
    public List<ILowLevelToken> Mantissa { get; }
    
    public override long StartIndex => Digits.First().StartIndex;
    public override long Length => Digits.Sum(d => d.Length) + DecimalPoint!.Length + Mantissa.Sum(d => d.Length);
    public override long EndIndex => Mantissa.Last().EndIndex;

    public DecimalToken()
    {
        Digits = new List<ILowLevelToken>();
        Mantissa = new List<ILowLevelToken>();
    }

    public decimal AsDecimal() => decimal.Parse(ToString());

    public override string ToString()
    {
        AssertComplete();

        return string.Concat(Digits) + DecimalPoint + string.Concat(Mantissa);
    }
}
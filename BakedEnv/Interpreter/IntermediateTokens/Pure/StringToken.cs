using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class StringToken : PureIntermediateToken
{
    public QuotationToken? LeftQuotation { get; set; }
    public List<StringContentToken> Content { get; }
    public QuotationToken? RightQuotation { get; set; }

    public StringToken()
    {
        Content = new List<StringContentToken>();
    }
}
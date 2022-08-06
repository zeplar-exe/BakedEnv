using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class StringToken : PureIntermediateToken
{
    public QuotationToken? LeftQuotation { get; set; }
    public List<StringContentToken> Content { get; }
    public QuotationToken? RightQuotation { get; set; }
    

    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            if (LeftQuotation != null) yield return LeftQuotation;

            foreach (var content in Content)
            {
                yield return content;
            }

            if (RightQuotation != null) yield return RightQuotation;
        }
    }

    public StringToken()
    {
        Content = new List<StringContentToken>();
    }
}
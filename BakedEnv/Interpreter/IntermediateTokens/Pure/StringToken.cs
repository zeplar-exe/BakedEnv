using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens.Interfaces;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class StringToken : PureIntermediateToken, IExpressionToken
{
    public QuotationToken? LeftQuotation { get; set; }
    public List<AnyToken> Content { get; }
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
        Content = new List<AnyToken>();
    }

    public BakedExpression CreateExpression()
    {
        AssertComplete();

        var concat = string.Concat(Content);
        
        var s = new BakedString(concat);

        return new ValueExpression(s);
    }
}
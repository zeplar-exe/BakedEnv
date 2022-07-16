using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public ValueExpressionParserResult BaseValueExpression { get; }
    public IEnumerable<BakedExpression> Chain { get; }
    public BakedExpression Expression { get; }
    
    public ExpressionParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        ValueExpressionParserResult baseValueExpression, 
        IEnumerable<BakedExpression> chain,
        BakedExpression expression) : base(allTokens)
    {
        IsComplete = complete;
        BaseValueExpression = baseValueExpression;
        Chain = chain;
        Expression = expression;
    }
    
    public class Builder : ResultBuilder
    {
        private ValueExpressionParserResult? BaseValueExpression { get; set; }
        private List<BakedExpression> Chain { get; }

        public Builder()
        {
            Chain = new List<BakedExpression>();
        }

        public Builder WithBaseExpression(ValueExpressionParserResult valueExpression)
        {
            BaseValueExpression = valueExpression;

            return this;
        }
        
        public Builder WithChain(IEnumerable<BakedExpression> expressions)
        {
            Chain.AddRange(expressions);

            return this;
        }

        public Builder WithChainExpression(BakedExpression expression)
        {
            Chain.Add(expression);

            return this;
        }
        
        public ExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            BuilderHelper.EnsurePropertyNotNull(BaseValueExpression);
            
            return new ExpressionParserResult(true, AllTokens, BaseValueExpression, Chain, expression);
        }

        public ExpressionParserResult BuildFailure()
        {
            BuilderHelper.EnsurePropertyNotNull(BaseValueExpression);
            
            return new ExpressionParserResult(false, AllTokens, BaseValueExpression, Chain, new NullExpression());
        }
    }
}
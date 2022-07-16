using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public ValueExpressionParserResult BaseValueExpression { get; }
    public BakedExpression Expression { get; }
    
    public ExpressionParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        ValueExpressionParserResult baseValueExpression, 
        BakedExpression expression) : base(allTokens)
    {
        IsComplete = complete;
        BaseValueExpression = baseValueExpression;
        Expression = expression;
    }
    
    public class Builder : ResultBuilder
    {
        private ValueExpressionParserResult BaseValueExpression { get; set; }

        public Builder WithBaseExpression(ValueExpressionParserResult valueExpression)
        {
            BaseValueExpression = valueExpression;

            return this;
        }
        
        public ExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new ExpressionParserResult(true, AllTokens, BaseValueExpression, expression);
        }

        public ExpressionParserResult BuildFailure()
        {
            return new ExpressionParserResult(false, AllTokens, BaseValueExpression, new NullExpression());
        }
    }
}
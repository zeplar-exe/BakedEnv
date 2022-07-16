using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class TailExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public ExpressionParserResult BaseExpression { get; }
    public BakedExpression Expression { get; }
    
    public TailExpressionParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        ExpressionParserResult baseExpression, 
        BakedExpression expression) : base(allTokens)
    {
        IsComplete = complete;
        BaseExpression = baseExpression;
        Expression = expression;
    }
    
    public class Builder : ResultBuilder
    {
        private ExpressionParserResult BaseExpression { get; set; }

        public Builder WithBaseExpression(ExpressionParserResult expression)
        {
            BaseExpression = expression;

            return this;
        }
        
        public TailExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new TailExpressionParserResult(true, AllTokens, BaseExpression, expression);
        }

        public TailExpressionParserResult BuildFailure()
        {
            return new TailExpressionParserResult(false, AllTokens, BaseExpression, new NullExpression());
        }
    }
}
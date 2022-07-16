using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ValueExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public BakedExpression Expression { get; }
    
    public ValueExpressionParserResult(bool complete, IEnumerable<LexerToken> allTokens, BakedExpression expression) : base(allTokens)
    {
        IsComplete = complete;
        Expression = expression;
    }

    public class Builder : ResultBuilder
    {

        public Builder WithToken(LexerToken token)
        {
            AddToken(token);

            return this;
        }
        
        public ValueExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new ValueExpressionParserResult(true, AllTokens, expression);
        }

        public ValueExpressionParserResult BuildFailure()
        {
            return new ValueExpressionParserResult(false, AllTokens, new NullExpression());
        }
    }
}
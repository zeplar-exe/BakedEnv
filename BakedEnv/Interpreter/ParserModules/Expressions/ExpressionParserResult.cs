using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public BakedExpression Expression { get; }
    
    public ExpressionParserResult(bool complete, IEnumerable<LexerToken> allTokens, BakedExpression expression) : base(allTokens)
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
        
        public ExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new ExpressionParserResult(true, AllTokens, expression);
        }

        public ExpressionParserResult BuildFailure()
        {
            return new ExpressionParserResult(false, AllTokens, new NullExpression());
        }
    }
}
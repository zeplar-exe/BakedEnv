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
    
    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private ExpressionParserResult BaseExpression { get; set; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }
        
        public Builder WithToken(LexerToken token)
        {
            Tokens.Add(token);

            return this;
        }

        public Builder WithBaseExpression(ExpressionParserResult expression)
        {
            BaseExpression = expression;

            return this;
        }

        public Builder WithTokens(IEnumerable<LexerToken> tokens)
        {
            Tokens.AddRange(tokens);

            return this;
        }
        
        public TailExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new TailExpressionParserResult(true, Tokens, BaseExpression, expression);
        }

        public TailExpressionParserResult BuildFailure()
        {
            return new TailExpressionParserResult(false, Tokens, BaseExpression, new NullExpression());
        }
    }
}
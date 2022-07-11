using BakedEnv.Interpreter.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class TailExpressionParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public BakedExpression Expression { get; }
    
    public TailExpressionParserResult(bool complete, IEnumerable<LexerToken> allTokens, BakedExpression expression) : base(allTokens)
    {
        IsComplete = complete;
        Expression = expression;
    }
    
    public class Builder
    {
        private List<LexerToken> Tokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }
        
        public Builder WithToken(LexerToken token)
        {
            Tokens.Add(token);

            return this;
        }

        public Builder WithTokens(IEnumerable<LexerToken> tokens)
        {
            Tokens.AddRange(tokens);

            return this;
        }
        
        public TailExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new TailExpressionParserResult(true, Tokens, expression);
        }

        public TailExpressionParserResult BuildFailure()
        {
            return new TailExpressionParserResult(false, Tokens, new NullExpression());
        }
    }
}
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
        
        public ExpressionParserResult BuildSuccess(BakedExpression expression)
        {
            return new ExpressionParserResult(true, Tokens, expression);
        }

        public ExpressionParserResult BuildFailure()
        {
            return new ExpressionParserResult(false, Tokens, new NullExpression());
        }
    }
}
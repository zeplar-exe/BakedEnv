using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class IndexerParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public TailExpressionParserResult Expression { get; }

    public IndexerParserResult(bool complete, IEnumerable<LexerToken> allTokens, 
        LexerToken openBracket,
        LexerToken closeBracket, 
        TailExpressionParserResult expression) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        Expression = expression;
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private TailExpressionParserResult? Expression { get; set; }

        public Builder WithOpening(LexerToken token)
        {
            OpenBracket = token;
            AddToken(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseBracket = token;
            AddToken(token);

            return this;
        }

        public Builder WithExpression(TailExpressionParserResult expression)
        {
            Expression = expression;
            AddTokensFrom(expression);
            
            return this;
        }

        public IndexerParserResult Build(bool complete)
        {
            BuilderHelper.EnsurePropertyNotNull(OpenBracket);
            BuilderHelper.EnsurePropertyNotNull(CloseBracket);
            BuilderHelper.EnsurePropertyNotNull(Expression);
            
            return new IndexerParserResult(complete, AllTokens, OpenBracket, CloseBracket, Expression);
        }
    }
}
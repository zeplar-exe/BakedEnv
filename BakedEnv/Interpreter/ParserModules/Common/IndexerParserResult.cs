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

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private LexerToken OpenBracket { get; set; }
        private LexerToken CloseBracket { get; set; }
        private TailExpressionParserResult Expression { get; set; }

        public Builder WithOpening(LexerToken token)
        {
            OpenBracket = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseBracket = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithExpression(TailExpressionParserResult expression)
        {
            Expression = expression;
            Tokens.AddRange(expression.AllTokens);
            
            return this;
        }

        public IndexerParserResult Build(bool complete)
        {
            return new IndexerParserResult(complete, Tokens, OpenBracket, CloseBracket, Expression);
        }
    }
}
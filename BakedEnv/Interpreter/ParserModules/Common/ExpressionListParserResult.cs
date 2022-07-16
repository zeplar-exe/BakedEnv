using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ExpressionListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<LexerToken> Separators { get; }
    public IEnumerable<TailExpressionParserResult> Expressions { get; }
    
    public ExpressionListParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens, 
        IEnumerable<LexerToken> separators,
        IEnumerable<TailExpressionParserResult> expressions) : base(allTokens)
    {
        IsComplete = complete;
        Separators = separators;
        Expressions = expressions;
    }

    public class Builder : ResultBuilder
    {
        private List<LexerToken> Separators { get; }
        private List<TailExpressionParserResult> Expressions { get; }

        public Builder()
        {
            
            Separators = new List<LexerToken>();
            Expressions = new List<TailExpressionParserResult>();
        }

        public Builder WithSeparator(LexerToken token)
        {
            Separators.Add(token);
            AddToken(token);

            return this;
        }

        public Builder WithTailExpression(TailExpressionParserResult expression)
        {
            Expressions.Add(expression);
            AddTokensFrom(expression);

            return this;
        }

        public ExpressionListParserResult Build(bool complete)
        {
            return new ExpressionListParserResult(complete, AllTokens, Separators, Expressions);
        }
    }
}
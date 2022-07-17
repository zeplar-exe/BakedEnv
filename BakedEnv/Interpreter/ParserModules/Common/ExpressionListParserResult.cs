using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ExpressionListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken[] Separators { get; }
    public ExpressionParserResult[] Expressions { get; }
    
    public ExpressionListParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens, 
        IEnumerable<LexerToken> separators,
        IEnumerable<ExpressionParserResult> expressions) : base(allTokens)
    {
        IsComplete = complete;
        Separators = separators.ToArray();
        Expressions = expressions.ToArray();
    }

    public class Builder : ResultBuilder
    {
        private List<LexerToken> Separators { get; }
        private List<ExpressionParserResult> Expressions { get; }

        public Builder()
        {
            
            Separators = new List<LexerToken>();
            Expressions = new List<ExpressionParserResult>();
        }

        public Builder WithSeparator(LexerToken token)
        {
            Separators.Add(token);
            AddToken(token);

            return this;
        }

        public Builder WithTailExpression(ExpressionParserResult expression)
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
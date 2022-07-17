using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.DataStructures;

internal class ArrayParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public ExpressionParserResult[] Expressions { get; }

    public ArrayParserResult(bool complete, IEnumerable<LexerToken> allTokens, 
        LexerToken openBracket,
        LexerToken closeBracket, 
        IEnumerable<ExpressionParserResult> expressions) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        Expressions = expressions.ToArray();
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private List<ExpressionParserResult> Expressions { get; set; }

        public Builder()
        {
            Expressions = new List<ExpressionParserResult>();
        }

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

        public Builder WithExpression(ExpressionParserResult expression)
        {
            Expressions.Add(expression);
            AddTokensFrom(expression);
            
            return this;
        }

        public ArrayParserResult Build(bool complete)
        {
            BuilderHelper.EnsurePropertyNotNull(OpenBracket);
            BuilderHelper.EnsurePropertyNotNull(CloseBracket);
            
            return new ArrayParserResult(complete, AllTokens, OpenBracket, CloseBracket, Expressions);
        }
    }
}
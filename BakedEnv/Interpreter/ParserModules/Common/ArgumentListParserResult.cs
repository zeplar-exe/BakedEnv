using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ArgumentListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenParenthesis { get; }
    public LexerToken CloseParenthesis { get; }
    public ExpressionListParserResult Expressions { get; }

    public ArgumentListParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        LexerToken openParenthesis,
        LexerToken closeParenthesis,
        ExpressionListParserResult expressions) : base(allTokens)
    {
        IsComplete = complete;
        OpenParenthesis = openParenthesis;
        CloseParenthesis = closeParenthesis;
        Expressions = expressions;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        public LexerToken? OpenParenthesis { get; set; }
        public LexerToken? CloseParenthesis { get; set; }
        private ExpressionListParserResult? Expressions { get; set; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithOpening(LexerToken token)
        {
            OpenParenthesis = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseParenthesis = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithExpressionList(ExpressionListParserResult result)
        {
            Tokens.AddRange(result.AllTokens);
            Expressions = result;

            return this;
        }

        public ArgumentListParserResult Build(bool complete)
        {
            BuilderHelper.EnsurePropertyNotNull(OpenParenthesis);
            BuilderHelper.EnsurePropertyNotNull(CloseParenthesis);
            BuilderHelper.EnsurePropertyNotNull(Expressions);
            
            return new ArgumentListParserResult(complete, Tokens, OpenParenthesis, CloseParenthesis, Expressions);
        }
    }
}
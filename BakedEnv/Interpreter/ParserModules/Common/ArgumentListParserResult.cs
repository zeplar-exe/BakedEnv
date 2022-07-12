using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ParameterListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenParenthesis { get; }
    public LexerToken CloseParenthesis { get; }
    public IEnumerable<LexerToken> ParameterSeparators { get; }
    public IEnumerable<TailExpressionParserResult> Parameters { get; }

    public ParameterListParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        LexerToken openParenthesis,
        LexerToken closeParenthesis,
        IEnumerable<LexerToken> parameterSeparators,
        IEnumerable<TailExpressionParserResult> parameters) : base(allTokens)
    {
        IsComplete = complete;
        OpenParenthesis = openParenthesis;
        CloseParenthesis = closeParenthesis;
        ParameterSeparators = parameterSeparators;
        Parameters = parameters;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        public LexerToken OpenParenthesis { get; set; }
        public LexerToken CloseParenthesis { get; set; }
        public List<LexerToken> Separators { get; }
        private List<TailExpressionParserResult> Parameters { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            Separators = new List<LexerToken>();
            Parameters = new List<TailExpressionParserResult>();
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

        public Builder WithTailExpression(TailExpressionParserResult result)
        {
            Tokens.AddRange(result.AllTokens);
            Parameters.Add(result);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            Tokens.Add(token);
            Separators.Add(token);

            return this;
        }

        public ParameterListParserResult Build(bool complete)
        {
            return new ParameterListParserResult(complete, Tokens, OpenParenthesis, CloseParenthesis, Separators, Parameters);
        }
    }
}
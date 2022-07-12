using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ParameterListParser : ParserModule
{
    public ParameterListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ParameterListParserResult Parse()
    {
        var builder = new ParameterListParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftParenthesis)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);

        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.RightParenthesis:
                {
                    return builder.WithClosing(token).Build(!expectValue);
                }
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }
                    
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    using var expressionParser = new TailExpressionParser(Internals);
                    var result = expressionParser.Parse();
                    
                    builder.WithTailExpression(result);

                    if (!result.IsComplete)
                    {
                        return builder.Build(false);
                    }
                    
                    expectValue = false;
                    
                    break;
                }
            }
        }

        return builder.Build(false);
    }
}

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
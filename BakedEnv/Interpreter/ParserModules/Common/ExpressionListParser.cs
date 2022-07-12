using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ExpressionListParser : ParserModule
{
    public ExpressionListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ExpressionListParserResult Parse()
    {
        var builder = new ExpressionListParserResult.Builder();
        
        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }

                    builder.WithSeparator(token);
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    Internals.Iterator.PushCurrent();

                    if (!expectValue)
                    {
                        return builder.Build(true);
                    }

                    var expressionParser = new TailExpressionParser(Internals);
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
            
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        return builder.Build(false);
    }
}

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

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
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
            Tokens.Add(token);

            return this;
        }

        public Builder WithTailExpression(TailExpressionParserResult expression)
        {
            Expressions.Add(expression);
            Tokens.AddRange(expression.AllTokens);

            return this;
        }

        public ExpressionListParserResult Build(bool complete)
        {
            return new ExpressionListParserResult(complete, Tokens, Separators, Expressions);
        }
    }
}
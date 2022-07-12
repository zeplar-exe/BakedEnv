using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ArithmeticParser : ParserModule
{
    public ArithmeticParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public BakedExpression Parse()
    {
        // has to include the expression and operator from before?
    }
}

internal class ArithmeticParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<ExpressionParserResult> Expressions { get; }
    public IEnumerable<LexerToken> Operators { get; }

    public ArithmeticParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        IEnumerable<ExpressionParserResult> expressions, 
        IEnumerable<LexerToken> operators) : base(allTokens)
    {
        IsComplete = complete;
        Expressions = expressions;
        Operators = operators;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<ExpressionParserResult> Expressions { get; }
        private List<LexerToken> Operators { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            Expressions = new List<ExpressionParserResult>();
            Operators = new List<LexerToken>();
        }

        public Builder WithExpression(ExpressionParserResult result)
        {
            Expressions.Add(result);
            Tokens.AddRange(result.AllTokens);

            return this;
        }

        public Builder WithOperator(LexerToken token)
        {
            Operators.Add(token);
            Tokens.Add(token);

            return this;
        }

        public ArithmeticParserResult Build(bool complete)
        {
            return new ArithmeticParserResult(complete, Tokens, Expressions, Operators);
        }
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ArithmeticParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<OperatorInfo> Operators { get; }
    public IEnumerable<TailExpressionParserResult> Expressions { get; }
    public IEnumerable<LexerToken> OperatorTokens { get; }

    public ArithmeticParserResult(
        bool complete,
        IEnumerable<OperatorInfo> operators,
        IEnumerable<LexerToken> allTokens,
        IEnumerable<TailExpressionParserResult> expressions, 
        IEnumerable<LexerToken> operatorTokens) : base(allTokens)
    {
        IsComplete = complete;
        Operators = operators;
        Expressions = expressions;
        OperatorTokens = operatorTokens;
    }

    public BakedExpression CreateExpression()
    {
        OperatorInfo? previous;
        
        foreach (var op in Operators)
        {
            

            previous = op;
        }

        return new NullExpression(); // TODO
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<OperatorInfo> Operators { get; }
        private List<TailExpressionParserResult> Expressions { get; }
        private List<LexerToken> OperatorTokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            Operators = new List<OperatorInfo>();
            Expressions = new List<TailExpressionParserResult>();
            OperatorTokens = new List<LexerToken>();
        }

        public Builder WithOperator(OperatorInfo info)
        {
            Operators.Add(info);
            Expressions.Add(info.Left);
            Expressions.Add(info.Right);
            Tokens.AddRange(info.AllTokens);

            return this;
        }

        public ArithmeticParserResult Build(bool complete)
        {
            return new ArithmeticParserResult(complete, Operators, Tokens, Expressions, OperatorTokens);
        }
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class OperatorChainEvaluator
{
    private Dictionary<LexerTokenType, int> PrecedenceTable { get; } = new()
    {
        { LexerTokenType.Plus, 0 },
        { LexerTokenType.Dash, 0 },
        { LexerTokenType.Star, 1 },
        { LexerTokenType.Slash, 1 },
        { LexerTokenType.Percent, 1 },
        { LexerTokenType.Caret, 2 },
    };
    
    private IEnumerator<OperatorInfo> Operators { get; }

    public OperatorChainEvaluator(IEnumerable<OperatorInfo> operators)
    {
        Operators = operators.GetEnumerator();
    }

    public BakedExpression Evaluate()
    {
        if (!Operators.MoveNext())
        {
            return new NullExpression();
        }

        var first = Operators.Current;

        return EvaluateNext(first.Left.Expression, first.Token);
    }

    private BakedExpression EvaluateNext(BakedExpression previousExpression, LexerToken previousToken)
    {
        if (!Operators.MoveNext())
        {
            return previousExpression;
        }

        var current = Operators.Current;

        if (TakesPrecedent(previousToken, current.Token))
        {
            var fullPreviousExpression = CreateExpression(previousToken, previousExpression, current.Left.Expression);
            var next = Evaluate();

            if (next is NullExpression)
                return fullPreviousExpression;

            return CreateExpression(current.Token, fullPreviousExpression, next);
        }
        else
        {
            var currentExpression = EvaluateNext(current.Left.Expression, current.Token);
            var fullCurrentExpression = CreateExpression(current.Token, current.Left.Expression, currentExpression);

            return CreateExpression(previousToken, previousExpression, fullCurrentExpression);
        }
    }

    private bool TakesPrecedent(LexerToken a, LexerToken b)
    {
        var precA = PrecedenceTable[a.Type];
        var precB = PrecedenceTable[b.Type];

        return precA > precB || precA == precB;
    }

    private BakedExpression CreateExpression(OperatorInfo info)
    {
        return CreateExpression(info.Token, info.Left.Expression, info.Right.Expression);
    }
    
    private BakedExpression CreateExpression(LexerToken token, BakedExpression left, BakedExpression right)
    {
        switch (token.Type)
        {
            case LexerTokenType.Plus: // Addition
            {
                return new AdditionExpression(left, right);
            }
            case LexerTokenType.Dash: // Subtraction
            {
                return new SubtractionExpression(left, right);
            }
            case LexerTokenType.Star: // Multiplication
            {
                return new MultiplicationExpression(left, right);
            }
            case LexerTokenType.Slash: // Division
            {
                return new DivisionExpression(left, right);
            }
            case LexerTokenType.Caret: // Exponentiation
            {
                return new ExponentiationExpression(left, right);
            }
            case LexerTokenType.Percent: // Modulus
            {
                return new ModulusExpression(left, right);
            }
            default:
            {
                return new NullExpression();
            }
        }
    }
}
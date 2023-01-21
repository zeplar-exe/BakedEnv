using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class FunctionInvocationParserNode : InterpreterParserNode
{
    
    private BakedExpression Expression { get; }
    private IntermediateToken ExpressionFirstToken { get; }

    public FunctionInvocationParserNode(BakedExpression expression, IntermediateToken expressionFirstToken)
    {
        Expression = expression;
        ExpressionFirstToken = expressionFirstToken;
    }

    public override DescendResult Descend(IntermediateToken token)
    {
        return DescendResult.SuccessfulIf(this, () => token is LeftParenthesisToken);
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var parameters = new List<BakedExpression>();

        var expectComma = false;

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
                return BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex).ToInstruction();

            if (next is RightParenthesisToken)
            {
                if (!expectComma)
                    parameters.Add(new NullExpression());
                
                break;
            }
            else if (next is CommaToken)
            {
                if (expectComma)
                {
                    expectComma = false;
                }
                else
                {
                    parameters.Add(new NullExpression());
                }
            } 
            else
            {
                if (expectComma)
                {
                    return BakedError.EExpectedArgumentDelimiter(next.GetType().Name, next.StartIndex).ToInstruction();
                }

                var expression = expressionParser.Parse(next, iterator, context, out var error);

                if (error != null)
                    return error.Value.ToInstruction();
                
                parameters.Add(expression);
                expectComma = true;
            }
        }

        return new ObjectInvocationInstruction(Expression, parameters.ToArray(), ExpressionFirstToken.StartIndex);
    }
}
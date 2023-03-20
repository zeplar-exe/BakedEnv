using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionListParser
{
    public OperationResult<BakedExpression[]> Parse(InterpreterIterator iterator, ParserContext context)
    {
        var expressions = new List<BakedExpression>();
        var expressionParser = new ExpressionParser();
        var expectComma = false;
        
        // Note that a non-expression (comma, right parens, etc) following a comma denotes an null value

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
            {
                var error = BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex);

                return OperationResult<BakedExpression[]>.Failure(error);
            }

            if (next is RightParenthesisToken)
            {
                if (expressions.Count == 0) // If no params, we don't need a null fill-in
                    break;
                
                if (!expectComma)
                    expressions.Add(new NullExpression());
                
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
                    expressions.Add(new NullExpression());
                }
            } 
            else
            {
                if (expectComma)
                {
                    var error = BakedError.EExpectedArgumentDelimiter(next.GetType().Name, next.StartIndex);

                    return OperationResult<BakedExpression[]>.Failure(error);
                }

                var expression = expressionParser.Parse(next, iterator, context);

                if (expression.HasError)
                    return OperationResult<BakedExpression[]>.Failure(expression.Error);
                
                expressions.Add(expression.Value);
                expectComma = true;
            }
        }

        return OperationResult<BakedExpression[]>.Success(expressions.ToArray());
    }
}
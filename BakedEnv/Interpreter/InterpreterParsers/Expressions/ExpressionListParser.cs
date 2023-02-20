using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionListParser
{
    public BakedExpression[] Parse(InterpreterIterator iterator, ParserContext context, out BakedError? error)
    {
        error = null;
        
        var expressions = new List<BakedExpression>();
        var expressionParser = new ExpressionParser();
        var expectComma = false;
        
        // Note that a non-expression (comma, right parens, etc) following a comma denotes an null value

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
            {
                error = BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex);

                return expressions.ToArray();
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
                    error = BakedError.EExpectedArgumentDelimiter(next.GetType().Name, next.StartIndex);

                    return expressions.ToArray();
                }

                var expression = expressionParser.Parse(next, iterator, context, out error);

                if (error != null)
                    return expressions.ToArray();
                
                expressions.Add(expression);
                expectComma = true;
            }
        }

        return expressions.ToArray();
    }
}
using BakedEnv.Interpreter.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionListParser
{
    public BakedExpression[] Parse(InterpreterIterator iterator, ParserContext context)
    {
        var expressions = new List<BakedExpression>();
        var expressionParser = new ExpressionParser();
        var expectComma = false;
        
        // Note that a non-expression (comma, right parens, etc) following a comma denotes an null value

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
            {
                BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex).Throw();
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
                    BakedError.EExpectedArgumentDelimiter(next.GetType().Name, next.StartIndex).Throw();
                }

                var expression = expressionParser.Parse(next, iterator, context);
                
                expressions.Add(expression);
                expectComma = true;
            }
        }

        return expressions.ToArray();
    }
}
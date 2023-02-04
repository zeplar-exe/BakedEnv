using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionListParser
{
    public BakedExpression[] Parse(InterpreterIterator iterator, ParserContext context, out BakedError? error)
    {
        error = null;
        
        var items = new List<BakedExpression>();
        var expressionParser = new ExpressionParser();
        var expectComma = false;

        while (true)
        {
            if (!iterator.TryMoveNext(out var next))
            {
                error = BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex);

                return items.ToArray();
            }

            if (next is RightParenthesisToken)
            {
                if (items.Count > 0 && !expectComma) // If no params, we don't need a null fill-in
                    items.Add(new NullExpression());
                
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
                    items.Add(new NullExpression());
                }
            } 
            else
            {
                if (expectComma)
                {
                    error = BakedError.EExpectedArgumentDelimiter(next.GetType().Name, next.StartIndex);

                    return items.ToArray();
                }

                var expression = expressionParser.Parse(next, iterator, context, out error);

                if (error != null)
                    return items.ToArray();
                
                items.Add(expression);
                expectComma = true;
            }
        }

        return items.ToArray();
    }
}
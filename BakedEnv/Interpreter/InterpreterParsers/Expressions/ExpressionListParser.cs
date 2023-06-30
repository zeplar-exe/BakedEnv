using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

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
            var next = iterator.MoveNextOrThrow();

            if (next.IsRawType(TextualTokenType.RightParenthesis))
            {
                if (expressions.Count == 0) // If no params, we don't need a null fill-in
                    break;
                
                if (!expectComma)
                    expressions.Add(new NullExpression());
                
                break;
            }
            else if (next.IsRawType(TextualTokenType.Comma))
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
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionParser
{
    public BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context, 
        out BakedError? error)
    {
        error = null;
        
        var selector = new ExpressionSelector();
        var parser = selector.SelectParser(first);

        if (parser == null)
        {
            error = BakedError.EUnknownExpression(first.GetType().Name, first.StartIndex);
            
            return new NullExpression();
        }

        var expression = parser.Parse(first, iterator, context);
        
        // check for parens and stuff here, then create invocation object

        return expression;
    }
}
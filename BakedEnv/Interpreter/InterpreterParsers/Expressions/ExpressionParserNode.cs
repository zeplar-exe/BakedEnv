using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        // what goes here?
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var selector = new ExpressionSelector();
        var parser = selector.SelectParser(first);

        if (parser == null)
        {
            return BakedError.EUnknownExpression(first.GetType().Name).ToInstruction();
        }

        var expression = parser.Parse(first, iterator, context);
        
        // be careful of incomplete tokens
    }
}
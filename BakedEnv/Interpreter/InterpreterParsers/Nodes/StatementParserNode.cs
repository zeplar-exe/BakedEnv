using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class StatementParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return DescendResult.Success(this);
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(first, iterator, context);
        var continuation = new StatementContinuationNode(expression, first);
        
        var next = iterator.MoveNextOrThrow();
        var descend = continuation.Descend(next);

        if (descend.IsSuccess)
            return descend.Parser.Parse(next, iterator, context);

        if (expression is InvocationExpression invocation)
            return new ObjectInvocationInstruction(invocation.Expression, invocation.Parameters, first.StartIndex);
        
        BakedError.EUnknownStatement(next.StartIndex).Throw();

        return new EmptyInstruction(0);
    }
}
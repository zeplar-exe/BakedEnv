using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class StatementParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return DescendResult.Successful(this);
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(first, iterator, context, out var error);

        if (error != null)
        {
            return error.Value.ToInstruction();
        }

        if (!TryMoveNext(iterator, out var next, out var nextError))
            return nextError.ToInstruction();

        var continuation = new StatementContinuationNode(expression, first);
        var descend = continuation.Descend(next);

        if (descend.Success)
            return descend.Parser.Parse(next, iterator, context);

        if (expression is InvocationExpression invocation)
            return new ObjectInvocationInstruction(invocation.Expression, invocation.Parameters, first.StartIndex);
        
        return BakedError.EUnknownStatement(next.StartIndex).ToInstruction();
    }
}
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class ProcessorStatementParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return token is LeftBracketToken ? DescendResult.Successful(this) : DescendResult.Failure();
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var leftBracket = (LeftBracketToken)first;

        if (!iterator.TryMoveNext(out var next))
            return BakedError.EEarlyEndOfFile(first.EndIndex).ToInstruction();
        
        var key = expressionParser.Parse(next, iterator, context, out var error);

        if (error != null)
            return error.Value.ToInstruction();

        if (!iterator.TryTakeNextOfType<ColonToken>(out var colonToken, out var invalid))
            return invalid;
        
        if (!iterator.TryMoveNext(out next))
            return BakedError.EEarlyEndOfFile(first.EndIndex).ToInstruction();

        var value = expressionParser.Parse(next, iterator, context, out error);
        
        if (error != null)
            return error.Value.ToInstruction();
        
        if (!iterator.TryTakeNextOfType<RightBracketToken>(out var rightBracket, out invalid))
            return invalid;

        return new ProcessorStatementInstruction(key, value, first.StartIndex);
    }
}
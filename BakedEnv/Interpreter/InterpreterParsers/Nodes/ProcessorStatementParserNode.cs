using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class ProcessorStatementParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return token.IsRawType(TextualTokenType.LeftBracket) ? DescendResult.Success(this) : DescendResult.Failure();
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();

        var next = iterator.MoveNextOrThrow();
        
        var keyExpression = expressionParser.Parse(next, iterator, context);

        if (!iterator.TryTakeNextRaw(TextualTokenType.Colon, out _, out var invalid))
            invalid.Throw();

        next = iterator.MoveNextOrThrow();

        var valueExpression = expressionParser.Parse(next, iterator, context);

        if (!iterator.TryTakeNextRaw(TextualTokenType.RightBracket, out _, out invalid))
            invalid.Throw();

        return new ProcessorStatementInstruction(keyExpression, valueExpression, first.StartIndex);
    }
}
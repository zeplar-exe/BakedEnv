using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Identifiers;

internal class IdentifierParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return token is IdentifierToken ? DescendResult.Successful(this) : DescendResult.Failure();
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator)
    {
        return new EmptyInstruction(first.StartIndex);
    }
}
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers;

internal class IdentifierParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        if (token is not IdentifierToken identifierToken)
        {
            return DescendResult.Successful(this);
        }

        return DescendResult.Failure();
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator)
    {
        return new EmptyInstruction(first.StartIndex);
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class IdentifierAccessParserNode : InterpreterParserNode
{
    private BakedExpression Previous { get; }

    public IdentifierAccessParserNode(BakedExpression previous)
    {
        Previous = previous;
    }

    public override DescendResult Descend(IntermediateToken token)
    {
        return token is IdentifierToken ? DescendResult.Successful(this) : DescendResult.Failure();
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        // Make this its own abstract class, like, ExpressionBranch idk
        return new MemberAccessExpression(Previous, first.ToString());
    }
}
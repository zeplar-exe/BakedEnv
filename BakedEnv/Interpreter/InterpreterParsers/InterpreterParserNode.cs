using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class InterpreterParserNode : BranchParser
{
    public abstract InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context);
}
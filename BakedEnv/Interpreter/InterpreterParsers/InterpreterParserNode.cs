using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class InterpreterParserNode : BranchParser
{
    public abstract InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context);
}
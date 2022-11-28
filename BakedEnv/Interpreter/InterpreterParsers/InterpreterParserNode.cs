using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class InterpreterParserNode : InterpreterParser
{
    public abstract InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator);
}
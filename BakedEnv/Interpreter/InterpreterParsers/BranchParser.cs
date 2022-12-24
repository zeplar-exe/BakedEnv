using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class BranchParser
{
    public abstract DescendResult Descend(IntermediateToken token);
}
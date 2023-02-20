using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class InterpreterParserNode : BranchParser
{
    public abstract InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context);

    protected bool TryMoveNext(InterpreterIterator iterator, 
        [NotNullWhen(true)] out IntermediateToken? next, 
        out BakedError error)
    {
        error = default;
        
        if (!iterator.TryMoveNext(out next))
        {
            error = BakedError.EEarlyEndOfFile(iterator.Current?.EndIndex ?? 0);

            return false;
        }

        return true;
    }
}
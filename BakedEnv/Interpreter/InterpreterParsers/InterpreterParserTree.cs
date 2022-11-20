using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

internal class InterpreterParserTree : InterpreterParser
{
    public TypeList<InterpreterParser> RootParsers { get; }

    public InterpreterParserTree()
    {
        RootParsers = new TypeList<InterpreterParser>();
    }
    
    public override DescendResult Descend(IntermediateToken token)
    {
        foreach (var parser in RootParsers.EnumerateInstances())
        {
            var result = parser.Descend(token);

            if (result.Success)
            {
                return result;
            }
        }
        
        return DescendResult.Failure();
    }
}
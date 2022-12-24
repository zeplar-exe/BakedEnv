using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public class InterpreterParserTree : BranchParser
{
    public TypeInstanceList<BranchParser> RootParserNodes { get; }

    public InterpreterParserTree()
    {
        RootParserNodes = new TypeInstanceList<BranchParser>();
    }
    
    public override DescendResult Descend(IntermediateToken token)
    {
        foreach (var parser in RootParserNodes.EnumerateInstances())
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
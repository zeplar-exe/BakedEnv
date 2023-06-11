using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public class InterpreterParserTree : BranchParser
{
    public List<BranchParser> RootParserNodes { get; }

    public InterpreterParserTree()
    {
        RootParserNodes = new List<BranchParser>();
    }
    
    public override DescendResult Descend(IntermediateToken token)
    {
        foreach (var parser in RootParserNodes)
        {
            var result = parser.Descend(token);

            if (result.IsSuccess)
            {
                return result;
            }
        }
        
        return DescendResult.Failure();
    }
}
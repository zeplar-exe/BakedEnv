using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers.Identifiers;

namespace BakedEnv.Interpreter.InterpreterParsers;

public class InterpreterParserTree : InterpreterParser
{
    public TypeList<InterpreterParser> RootParserNodes { get; }

    public InterpreterParserTree()
    {
        RootParserNodes = new TypeList<InterpreterParser>();

        RootParserNodes.Add<IdentifierParserNode>();
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
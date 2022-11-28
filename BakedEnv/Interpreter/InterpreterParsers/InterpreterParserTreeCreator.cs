using BakedEnv.Common;
using BakedEnv.Interpreter.InterpreterParsers.Identifiers;

namespace BakedEnv.Interpreter.InterpreterParsers;

public class InterpreterParserTreeCreator
{
    public TypeList<InterpreterParser> Parsers { get; }

    public InterpreterParserTreeCreator()
    {
        Parsers = new TypeList<InterpreterParser>();
    }

    public static InterpreterParserTreeCreator Default()
    {
        var creator = new InterpreterParserTreeCreator();

        creator.Parsers.Add<IdentifierParserNode>();

        return creator;
    }

    public InterpreterParserTree Create()
    {
        var tree = new InterpreterParserTree();
        tree.RootParserNodes.AddFrom(Parsers);
        
        return tree;
    }
}
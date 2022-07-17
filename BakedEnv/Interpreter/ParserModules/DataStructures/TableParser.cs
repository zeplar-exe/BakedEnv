using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.DataStructures;

internal class TableParser : ParserModule
{
    public TableParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public TableParserResult Parse()
    {
        
    }
}

internal class TableParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public KeyValueParserResult[] KeyValuePairs { get; }
    
    public TableParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        IEnumerable<KeyValueParserResult> keyValuePairs) : base(allTokens)
    {
        IsComplete = complete;
        KeyValuePairs = keyValuePairs.ToArray();
    }

    public class Builder : ResultBuilder
    {
        
    }
}
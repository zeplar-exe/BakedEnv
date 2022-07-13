using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class NameListParser : ParserModule
{
    public NameListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public NameListParserResult Parse()
    {
        
    }
}

internal class NameListParserResult : ParserModuleResult
{
    public IEnumerable<SingleIdentifierResult> Names { get; }

    public NameListParserResult(IEnumerable<LexerToken> allTokens) : base(allTokens)
    {
        
    }

    public class Builder
    {
        
    }
}
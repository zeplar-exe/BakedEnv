using TokenCs;

namespace BakedEnv.Interpreter.ParserModules;

internal class ParserModuleResult
{
    public IEnumerable<LexerToken> AllTokens { get; }

    public ParserModuleResult(IEnumerable<LexerToken> allTokens)
    {
        AllTokens = allTokens;
    }
}
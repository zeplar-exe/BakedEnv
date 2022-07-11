using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class SingleIdentifierResult : ParserModuleResult
{
    public bool IsEmpty => string.IsNullOrEmpty(Identifier);
    public string Identifier { get; }
    
    public SingleIdentifierResult(IEnumerable<LexerToken> allTokens) : base(allTokens)
    {
        Identifier = string.Join("", AllTokens.Select(t => t.ToString()));
    }
}
using BakedEnv.Interpreter.Variables;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class SingleIdentifierParserResult : ParserModuleResult
{
    public bool IsEmpty => string.IsNullOrEmpty(Identifier);
    public string Identifier { get; }
    
    public SingleIdentifierParserResult(IEnumerable<LexerToken> allTokens) : base(allTokens)
    {
        Identifier = string.Join("", AllTokens.Select(t => t.ToString()));
    }
    
    public VariableReference CreateReference(BakedInterpreter interpreter)
    {
        return new VariableReference(Identifier, interpreter);
    }
}
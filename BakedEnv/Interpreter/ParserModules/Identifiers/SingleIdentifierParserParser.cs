using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class SingleIdentifierParser : ParserModule
{
    public SingleIdentifierParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public SingleIdentifierParserResult Parse()
    {
        return new SingleIdentifierParserResult(ParseParts());
    }
    
    private IEnumerable<LexerToken> ParseParts()
    {
        var yieldCount = 0;
        
        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.AlphaNumeric:
                case LexerTokenType.Numeric when yieldCount > 0:
                case LexerTokenType.Underscore:
                    yield return token;
                    yieldCount++;
                    break;
                default:
                    Internals.Iterator.PushCurrent();
                    yield break;
            }
        }
    }
}
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class ChainIdentifierParser : ParserModule
{
    public ChainIdentifierParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ChainIdentifierParserResult Parse()
    {
        var expectIdentifier = true;
        var builder = new ChainIdentifierParserResult.Builder();
        
        LexerToken? token = null;
        // Damn, was hoping the else statement would guarantee token is initialized

        while (expectIdentifier || Internals.Iterator.TryMoveNext(out token))
            // If we're expecting an identifier, we don't need to take the next token
        {
            if (expectIdentifier)
            {
                var identifierParser = new SingleIdentifierParser(Internals);
                var identifier = identifierParser.Parse();

                if (identifier.IsEmpty)
                {
                    return builder.Build(false);
                }

                builder.WithName(identifier);
                expectIdentifier = false;
            }
            else
            {
                if (token!.Type is not LexerTokenType.Period)
                {
                    Internals.Iterator.PushCurrent();
                    
                    break;
                }

                builder.WithSeparator(token);
                expectIdentifier = true;
            }

            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        if (expectIdentifier) // End of file
        {
            return builder.Build(false);
        }

        return builder.Build(true);
    }
}
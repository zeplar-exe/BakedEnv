using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class NameListParser : ParserModule
{
    public NameListParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public NameListParserResult Parse()
    {
        var builder = new NameListParserResult.Builder();

        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }

                    builder.WithSeparator(token);
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    Internals.Iterator.PushCurrent();

                    if (!expectValue)
                    {
                        return builder.Build(true);
                    }

                    var identifierParser = new SingleIdentifierParser(Internals);
                    var result = identifierParser.Parse();
                    
                    builder.WithIdentifier(result);

                    if (result.IsEmpty)
                    {
                        return builder.Build(false);
                    }

                    expectValue = false;
                    
                    break;
                }
            }
            
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        return builder.Build(false);
    }
}
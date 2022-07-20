using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Misc;

internal class ProcessorParser : ParserModule
{
    public ProcessorParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ProcessorParserResult Parse()
    {
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftBracket)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var pairParser = new KeyValueParser(Internals);
        var pairResult = pairParser.Parse();

        if (!pairResult.IsComplete)
        {
            return builder.Build(false);
        }

        builder.WithKeyValue(pairResult);
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        if (!Internals.Iterator.TryMoveNext(out var closing))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.RightBracket)
        {
            return builder.Build(false);
        }

        return builder.WithClosing(closing).Build(true);
    }
}

internal class ProcessorParserResult : ParserModuleResult
{
    public ProcessorParserResult(IEnumerable<LexerToken> allTokens) : base(allTokens)
    {
        
    }

    public class Builder : ResultBuilder
    {
        
    }
}
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class TableParser : ParserModule
{
    public TableParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public TableParserResult Parse()
    {
        var builder = new TableParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftCurlyBracket)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            if (token.Type == LexerTokenType.RightCurlyBracket)
            {
                return builder.WithClosing(token).Build(true);
            }
            
            Internals.Iterator.PushCurrent();
            
            var expressionParser = new KeyValueParser(Internals);
            var result = expressionParser.Parse();

            builder.WithKeyValuePair(result);
        }

        return builder.Build(false);
    }
}
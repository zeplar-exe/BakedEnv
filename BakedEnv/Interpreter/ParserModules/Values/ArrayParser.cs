using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class ArrayParser : ParserModule
{
    public ArrayParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ArrayParserResult Parse()
    {
        var builder = new ArrayParserResult.Builder();
        
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
        
        var expressionParser = new ExpressionListParser(Internals);
        var result = expressionParser.Parse();

        builder.WithExpressions(result);

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (!Internals.Iterator.TryPeekNext(out var token))
        {
            return builder.Build(false);
        }

        if (token.Type != LexerTokenType.RightBracket)
        {
            return builder.Build(false);
        }

        return builder.WithClosing(token).Build(true);
    }
}
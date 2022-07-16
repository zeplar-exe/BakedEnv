using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class IndexerParser : ParserModule
{
    public IndexerParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public IndexerParserResult Parse()
    {
        var builder = new IndexerParserResult.Builder();
        
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
        
        var expressionParser = new TailExpressionParser(Internals);
        var result = expressionParser.Parse();

        builder.WithExpression(result);

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
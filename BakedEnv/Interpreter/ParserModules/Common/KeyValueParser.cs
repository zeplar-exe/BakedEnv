using BakedEnv.Interpreter.Parsers;
using TokenCs;
using ExpressionParser = BakedEnv.Interpreter.ParserModules.Expressions.ExpressionParser;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class KeyValueParser : ParserModule
{
    public KeyValueParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public KeyValueParserResult Parse()
    {
        var builder = new KeyValueParserResult.Builder();
        
        var expressionParser = new ExpressionParser(Internals);
        var expressionResult = expressionParser.Parse();

        builder.WithKey(expressionResult);
        
        if (!expressionResult.IsComplete)
        {
            return builder.Build(false);
        }

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (!Internals.Iterator.TryMoveNext(out var next))
        {
            return builder.Build(false);
        }

        if (next.Type != LexerTokenType.Colon)
        {
            return builder.Build(false);
        }

        builder.WithSeparator(next);
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        expressionResult = expressionParser.Parse();

        builder.WithValue(expressionResult);
        
        if (!expressionResult.IsComplete)
        {
            return builder.Build(false);
        }

        return builder.Build(true);
    }
}
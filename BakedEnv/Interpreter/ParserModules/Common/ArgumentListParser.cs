using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ArgumentListParser : ParserModule
{
    public ArgumentListParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ArgumentListParserResult Parse()
    {
        var builder = new ArgumentListParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftParenthesis)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        var expressionParser = new ExpressionListParser(Internals);
        var result = expressionParser.Parse();
                    
        builder.WithExpressionList(result);

        if (!result.IsComplete)
        {
            return builder.Build(false);
        }
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        if (!Internals.Iterator.TryPeekNext(out var last))
        {
            return builder.Build(false);
        }

        if (last.Type != LexerTokenType.RightParenthesis)
        {
            return builder.Build(false);
        }

        return builder.WithClosing(last).Build(true);
    }
}
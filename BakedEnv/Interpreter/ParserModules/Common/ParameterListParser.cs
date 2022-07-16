using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ParameterListParser : ParserModule
{
    public ParameterListParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ParameterListParserResult Parse()
    {
        var builder = new ParameterListParserResult.Builder();
        
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
        
        var nameParser = new NameListParser(Internals);
        var result = nameParser.Parse();
                    
        builder.WithNameList(result);

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
using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ArgumentListParser : ParserModule
{
    public ArgumentListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ParameterListParserResult Parse()
    {
        var builder = new ParameterListParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
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
        
        if (Internals.TestEndOfFile(out var last, out eofResult))
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
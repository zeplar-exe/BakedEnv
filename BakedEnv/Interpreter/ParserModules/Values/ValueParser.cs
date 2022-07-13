using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class ValueParser : ParserModule
{
    public ValueParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ValueParserResult Parse()
    {
        var builder = new ValueParserResult.Builder();
        
        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.Numeric:
            {
                var numericParser = new NumericParser(Internals);
                var result = numericParser.Parse();

                builder.WithTokens(result.AllTokens);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(result.Value);
            }
            case LexerTokenType.SingleQuotation:
            case LexerTokenType.DoubleQuotation:
            {
                var stringParser = new StringParser(Internals);
                var result = stringParser.Parse();

                builder.WithTokens(result.AllTokens);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new BakedString(result.String));
            }
        }
        
        return builder.BuildFailure();
    }
}
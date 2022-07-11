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
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.Numeric:
            {
                Internals.Iterator.PushCurrent();
                
                break;
            }
            case LexerTokenType.DoubleQuotation:
            {
                Internals.Iterator.PushCurrent();

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
using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;
using TokenCs.Extensions;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class NumericParser : ParserModule
{
    public NumericParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public NumericParserResult Parse()
    {
        var builder = new NumericParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.BuildFailure();
        }

        if (first.Type != LexerTokenType.Numeric)
        {
            return builder.BuildFailure();
        }
        
        builder.WithToken(first);

        if (first.IsNumeric())
        {
            if (int.TryParse(first.ToString(), out var i))
            {
                return builder.BuildSuccess(new BakedInteger(i));
            }
        }
        else if (first.IsDecimal())
        {
            if (double.TryParse(first.ToString(), out var d))
            {
                return builder.BuildSuccess(new BakedInteger(d));
            }
        }
        
        return builder.BuildFailure();
    }
}
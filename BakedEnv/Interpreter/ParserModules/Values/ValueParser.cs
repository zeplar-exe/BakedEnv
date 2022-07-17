using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class ValueParser : ParserModule
{
    public ValueParser(ParserEnvironment internals) : base(internals)
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
            case LexerTokenType.AlphaNumeric: // Function
            {
                if (first.ToString() == FunctionExpressionParser.Keyword)
                {
                    var functionParser = new FunctionExpressionParser(Internals);
                    var functionResult = functionParser.Parse();

                    if (functionResult.IsDeclaration)
                    {
                        builder.AddTokensFrom(functionResult);
                        
                        if (!functionResult.IsComplete)
                        {
                            return builder.BuildFailure();
                        }

                        return builder.BuildSuccess(functionResult.Function);
                    }
                }

                return builder.BuildFailure();
            }
            case LexerTokenType.Numeric: // Number
            {
                var numericParser = new NumericParser(Internals);
                var result = numericParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(result.Value);
            }
            case LexerTokenType.SingleQuotation: // String
            case LexerTokenType.DoubleQuotation:
            {
                var stringParser = new StringParser(Internals);
                var result = stringParser.Parse();

                builder.AddTokensFrom(result);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new BakedString(result.String));
            }
            case LexerTokenType.LeftBracket: // Array
            {
                var arrayParser = new ArrayParser(Internals);
                var arrayResult = arrayParser.Parse();

                if (!arrayResult.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return new BakedArray(); // TODO: Array declaration instruction, same goes for other values
            }
            case LexerTokenType.LeftCurlyBracket: // Table
            {
                break;
            }
        }
        
        return builder.BuildFailure();
    }
}
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ParameterParser
{
    private BakedInterpreter Interpreter { get; }
    private InterpreterIterator Iterator { get; }
    
    public ParameterParser(BakedInterpreter interpreter, InterpreterIterator iterator)
    {
        Interpreter = interpreter;
        Iterator = iterator;
    }

    public TryResult TryParseParameterList(out BakedObject[] parameters)
    {
        var list = new List<BakedObject>();
        var valueExpected = true;

        while (Iterator.TryMoveNext(out var next))
        {
            switch (next.Id)
            {
                case LexerTokenId.RightParenthesis:
                    parameters = list.ToArray();
                    return new TryResult(true);
                case LexerTokenId.Comma:
                    if (valueExpected)
                        list.Add(new BakedNull());

                    valueExpected = true;
                    break;
                case LexerTokenId.Whitespace:
                case LexerTokenId.Newline:
                    break;
                case LexerTokenId.Alphabetic:
                case LexerTokenId.AlphaNumeric:
                case LexerTokenId.Numeric:
                    var valueParser = Interpreter.CreateValueParser();
                    var valueResult = valueParser.TryParseValue(out var parameter);

                    if (!valueResult.Success)
                    {
                        parameters = Array.Empty<BakedObject>();

                        return valueResult with { Success = false };
                    }

                    list.Add(parameter);
                    valueExpected = false;

                    switch (Iterator.Current.Id)
                    {
                        case LexerTokenId.RightParenthesis:
                            parameters = list.ToArray();
                            return new TryResult(true);
                        case LexerTokenId.Comma:
                            valueExpected = true;
                            break;
                    }

                    break;
            }
        }

        parameters = list.ToArray();

        return new TryResult(true);
    }
}
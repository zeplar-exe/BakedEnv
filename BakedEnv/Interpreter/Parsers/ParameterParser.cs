using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ParameterParser
{
    private InterpreterInternals Internals { get; }

    public ParameterParser(InterpreterInternals internals)
    {
        Internals = internals;
    }

    public TryResult TryParseParameterList(out BakedObject[] parameters)
    {
        var list = new List<BakedObject>();
        var valueExpected = true;

        while (Internals.Iterator.TryMoveNext(out var next))
        {
            switch (next.Id)
            {
                case LexerTokenId.RightParenthesis:
                    if (valueExpected && list.Count > 0)
                        list.Add(new BakedNull());
                    
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
                default:
                    Internals.Iterator.PushCurrent();
                    var valueParser = Internals.Interpreter.CreateValueParser();
                    var valueResult = valueParser.TryParseValue(out var parameter);

                    if (!valueResult.Success)
                    {
                        parameters = Array.Empty<BakedObject>();

                        return valueResult;
                    }

                    list.Add(parameter);
                    valueExpected = false;

                    if (!Internals.Iterator.TryMoveNext(out var token))
                    {
                        parameters = Array.Empty<BakedObject>();
                        
                        return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
                    }

                    switch (token.Id)
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
        
        return Internals.Iterator.AtEnd ? 
            Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current) :
            new TryResult(true);

    }
}
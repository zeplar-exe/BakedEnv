using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;
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

    public TryResult TryParseParameterList(out BakedExpression[] parameters)
    {
        var list = new List<BakedExpression>();
        var valueExpected = true;

        while (Internals.Iterator.TryMoveNext(out var next))
        {
            switch (next.Id)
            {
                case LexerTokenId.RightParenthesis:
                    if (valueExpected && list.Count > 0)
                        list.Add(new NullExpression());
                    
                    parameters = list.ToArray();
                    
                    return new TryResult(true);
                case LexerTokenId.Comma:
                    if (valueExpected)
                        list.Add(new NullExpression());

                    valueExpected = true;
                    break;
                case LexerTokenId.Whitespace:
                case LexerTokenId.Newline:
                    break;
                default:
                    Internals.Iterator.PushCurrent();
                    var expressionParser = Internals.Interpreter.CreateExpressionParser();
                    var expressionResult = expressionParser.TryParseExpression(out var parameter);

                    if (!expressionResult.Success)
                    {
                        parameters = Array.Empty<BakedExpression>();

                        return expressionResult;
                    }

                    list.Add(parameter);
                    valueExpected = false;

                    if (!Internals.Iterator.TryMoveNext(out var token))
                    {
                        parameters = Array.Empty<BakedExpression>();
                        
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
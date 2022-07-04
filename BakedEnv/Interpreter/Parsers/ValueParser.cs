using System.Text;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ValueParser
{
    private InterpreterInternals Internals { get; }
    
    public ValueParser(InterpreterInternals internals)
    {
        Internals = internals;
    }

    public TryResult TryParseValue(out BakedObject value)
    {
        value = new BakedNull();

        if (!Internals.Iterator.TryMoveNext(out var startToken))
            return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
        
        switch (startToken.Id)
        {
            case LexerTokenId.Alphabetic: // Variable
            case LexerTokenId.AlphaNumeric:
            {
                var identifierResult = TryParseIdentifier(out var path);
                
                if (!identifierResult.Success)
                    return identifierResult;

                var reference = GetVariableReference(path);
                
                if (reference.TryGetVariable(out var variable))
                {
                    value = variable.Value;
                    
                    return new TryResult(true);
                }

                return new TryResult(false, 
                    new BakedError(
                        null, 
                        $"Variable, variable path, or part of path " + 
                        $"'{string.Join(".", path.AsEnumerable())}' does not exist.",
                        startToken.Span.Start));
            }
            case LexerTokenId.Numeric: // Number
            {
                value = new BakedInteger(startToken.ToString());

                return new TryResult(true);
            }
            case LexerTokenId.Quote: // String
            case LexerTokenId.DoubleQuote:
            {
                var quoteType = Internals.Iterator.Current.Id;
                var builder = new StringBuilder();
                var escaped = false;
                var completed = false;
                
                while (Internals.Iterator.TryMoveNext(out var next))
                {
                    if (next.Id == quoteType && !escaped)
                    {
                        completed = true;
                        break;
                    }

                    switch (next.Id, next.ToString(), escaped)
                    {
                        case (LexerTokenId.Alphabetic, "n", true): builder.Append('\n'); break; // new line
                        case (LexerTokenId.Alphabetic, "r", true): builder.Append('\r'); break; // carriage return
                        case (LexerTokenId.Alphabetic, "t", true): builder.Append('\t'); break; // horizontal tab
                        case (LexerTokenId.Alphabetic, "a", true): builder.Append('\a'); break; // bell
                        case (LexerTokenId.Alphabetic, "f", true): builder.Append('\f'); break; // form
                        case (LexerTokenId.Alphabetic, "v", true): builder.Append('\v'); break; // vertical tab
                        case (LexerTokenId.Backslash, "\\", true): builder.Append('\\'); break; // backslash
                        default:
                            if (escaped) // if nothing was escaped, append backslash
                                builder.Append('\\');
                            
                            builder.Append(next);
                            break;
                    }
                    
                    escaped = !escaped && next.Is(LexerTokenId.Backslash);
                }

                if (!completed)
                {
                    return new TryResult(false, Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
                }

                value = new BakedString(builder.ToString());

                return new TryResult(true);
            }
            case LexerTokenId.OpenBracket: // Table
            {
                var table = new BakedTable();
                var valueExpected = true;
                
                while (!Internals.Iterator.Current.Is(LexerTokenId.CloseBracket))
                {
                    Internals.IteratorTools.SkipWhitespaceAndNewlines();
                    
                    if (!Internals.Iterator.TryMoveNext(out var token))
                        return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);

                    if (token.Is(LexerTokenId.CloseBracket)) 
                    {
                        break;
                    }
                    else
                    {
                        if (!valueExpected)
                        {
                            return new TryResult(false, 
                                Internals.ErrorReporter.ReportUnexpectedTokenType(token, LexerTokenId.CloseBracket));
                        }
                        
                        Internals.Iterator.PushCurrent();
                    }

                    var keyParse = TryParseValue(out var keyValue);

                    if (!keyParse.Success)
                    {
                        return keyParse;
                    }

                    Internals.IteratorTools.SkipWhitespaceAndNewlines();

                    if (!Internals.Iterator.TryMoveNext(out var next))
                        return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
                    
                    if (Internals.ErrorReporter.TestUnexpectedTokenType(next, out var error, LexerTokenId.Colon))
                    {
                        return new TryResult(false, error);
                    }

                    Internals.IteratorTools.SkipWhitespaceAndNewlines();
                    
                    var valueParse = TryParseValue(out var valueObject);

                    if (!valueParse.Success)
                    {
                        return keyParse;
                    }
                    
                    Internals.IteratorTools.SkipWhitespaceAndNewlines();
                    
                    if (Internals.Iterator.TryMoveNext(out var pairSeparator))
                    {
                        valueExpected = pairSeparator.Is(LexerTokenId.Comma);
                    }
                    else
                    {
                        return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
                    }

                    table[keyValue] = valueObject;
                }

                value = table;

                return new TryResult(true);
            }
        }

        
        return new TryResult(false, Internals.ErrorReporter.ReportInvalidValue(startToken));
    }

    public TryResult TryParseIdentifier(out LexerToken[] path)
    {
        path = Array.Empty<LexerToken>();

        if (Internals.ErrorReporter.TestUnexpectedTokenType(Internals.Iterator.Current, out var currentError,
                LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
        {
            return new TryResult(false, currentError);
        }
        
        var pathList = new List<LexerToken> { Internals.Iterator.Current };

        if (!Internals.Iterator.TryMoveNext(out var next))
        {
            return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
        }
        
        if (next.Is(LexerTokenId.Period))
        {
            var parseNextIdentifier = true;

            while (Internals.Iterator.TryMoveNext(out next))
            {
                if (parseNextIdentifier)
                {
                    if (Internals.ErrorReporter.TestUnexpectedTokenType(next, out var error,
                            LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
                    {
                        return new TryResult(false, error);
                    }
                    
                    pathList.Add(next);

                    parseNextIdentifier = false;
                }
                else
                {
                    if (next.Id is not LexerTokenId.Period)
                    {
                        break;
                    }

                    parseNextIdentifier = true;
                }
            }

            if (parseNextIdentifier)
            {
                return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
            }
        }
        else
        {
            Internals.Iterator.PushCurrent();
        }
        
        path = pathList.ToArray();

        return new TryResult(true);
    }
    
    public VariableReference GetVariableReference(LexerToken[] path)
    {
        return new VariableReference(path.Select(c => c.ToString()), Internals.Interpreter, Internals.Scope);
    }
}
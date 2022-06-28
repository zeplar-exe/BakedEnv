using System.Text;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter.Parsers;

internal class ValueParser
{
    private BakedInterpreter Interpreter { get; }
    private EnumerableIterator<LexerToken> Iterator { get; }
    private IteratorTools IteratorTools { get; }
    private IBakedScope Scope { get; }
    private CommonErrorReporter ErrorReporter { get; }
    
    public ValueParser(BakedInterpreter interpreter, EnumerableIterator<LexerToken> iterator, IteratorTools iteratorTools, IBakedScope scope, CommonErrorReporter errorReporter)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        Scope = scope;
        ErrorReporter = errorReporter;
    }

    public TryResult TryParseValue(out BakedObject value)
    {
        value = new BakedNull();
        var startToken = Iterator.Current;
        
        switch (Iterator.Current.Id)
        {
            case LexerTokenId.Alphabetic: // Variable
            case LexerTokenId.AlphaNumeric:
            {
                var identifierParser = Interpreter.CreateIdentifierParser();
                var identifierResult = identifierParser.TryParseIdentifier(out var path);
                
                if (!identifierResult.Success)
                    return identifierResult;

                var reference = identifierParser.GetVariableReference(path, Scope);
                
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
                value = new BakedInteger(Iterator.Current.ToString());

                return new TryResult(true);
            }
            case LexerTokenId.Quote: // String
            case LexerTokenId.DoubleQuote:
            {
                var quoteType = Iterator.Current.Id;
                var builder = new StringBuilder();
                var escaped = false;
                
                while (Iterator.TryMoveNext(out var next))
                {
                    if (next.Id == quoteType && !escaped)
                    {
                        Iterator.Skip();
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

                value = new BakedString(builder.ToString());

                return new TryResult(true);
            }
            case LexerTokenId.OpenBracket: // Table
            {
                var table = new BakedTable();
                
                IteratorTools.SkipWhitespaceAndNewlines();

                var valueExpected = false;

                while (!Iterator.Current.Is(LexerTokenId.CloseBracket))
                {
                    if (valueExpected)
                    {
                        return new TryResult(false, 
                            ErrorReporter.ReportUnexpectedTokenType(Iterator.Current, 
                                LexerTokenId.CloseBracket));
                    }

                    IteratorTools.SkipWhitespaceAndNewlinesReserved();
                    
                    var keyParse = TryParseValue(out var keyValue);

                    if (!keyParse.Success)
                    {
                        return keyParse with { Success = false };
                    }

                    IteratorTools.SkipWhitespaceAndNewlines();

                    if (ErrorReporter.TestUnexpectedTokenType(Iterator.Current, out var error,
                            LexerTokenId.Colon))
                    {
                        return new TryResult(false, error);
                    }

                    IteratorTools.SkipWhitespaceAndNewlines();
                    
                    var valueParse = TryParseValue(out var valueObject);

                    if (!valueParse.Success)
                    {
                        return keyParse with { Success = false };
                    }
                    
                    IteratorTools.SkipWhitespaceAndNewlines();
                    
                    if (Iterator.TryMoveNext(out var pairSeparator) && !pairSeparator.Is(LexerTokenId.Comma))
                    {
                        valueExpected = true;
                    }
                    
                    IteratorTools.SkipWhitespaceAndNewlines();

                    table[keyValue] = valueObject;
                }

                value = table;

                return new TryResult(true);
            }
        }

        return new TryResult(false, ErrorReporter.ReportInvalidValue(startToken));
    }
}
using BakedEnv.Common;
using BakedEnv.Interpreter.Variables;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter.Parsers;

internal class IdentifierParser
{
    private BakedInterpreter Interpreter { get; }
    private CommonErrorReporter ErrorReporter { get; }
    private IteratorTools IteratorTools { get; }
    private EnumerableIterator<LexerToken> Iterator { get; }
    
    public IdentifierParser(BakedInterpreter interpreter, CommonErrorReporter errorReporter, IteratorTools iteratorTools, EnumerableIterator<LexerToken> iterator)
    {
        Interpreter = interpreter;
        ErrorReporter = errorReporter;
        IteratorTools = iteratorTools;
        Iterator = iterator;
    }
    
    public TryResult TryParseIdentifier(out LexerToken[] path)
    {
        path = Array.Empty<LexerToken>();
        
        var pathList = new List<LexerToken> { Iterator.Current };
        
        if (Iterator.TryMoveNext(out var next) && next.Is(LexerTokenId.Period))
        {
            var parseNextIdentifier = true;

            while (Iterator.TryMoveNext(out next))
            {
                IteratorTools.SkipWhitespaceAndNewlines();
                
                if (parseNextIdentifier)
                {
                    if (ErrorReporter.TestUnexpectedTokenType(next, out var error,
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
        }
        
        path = pathList.ToArray();

        return new TryResult(true);
    }
    
    public VariableReference GetVariableReference(LexerToken[] path, IBakedScope scope)
    {
        return new VariableReference(path.Select(c => c.ToString()), Interpreter, scope);
    }
}
using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Variables;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter.Parsers;

internal class InterpreterInternals
{
    public BakedInterpreter Interpreter { get; }
    public InterpreterIterator Iterator { get; }
    public IteratorTools IteratorTools { get; }
    public CommonErrorReporter ErrorReporter { get; }
    public StateMachine<ParserState> State { get; }
    public IBakedScope Scope { get; }
    
    public InterpreterInternals(
        BakedInterpreter interpreter, 
        InterpreterIterator iterator, 
        IteratorTools iteratorTools, 
        CommonErrorReporter errorReporter, 
        StateMachine<ParserState> state, 
        IBakedScope scope)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        ErrorReporter = errorReporter;
        State = state;
        Scope = scope;
    }

    public bool TestEndOfFile([NotNullWhen(false)] out LexerToken? token, out TryResult result)
    {
        result = default;
        
        var isTrue = false;
        
        if (!Iterator.TryMoveNext(out token))
        {
            result = ErrorReporter.EndOfFileResult(Iterator.Current);
            isTrue = true;
        }

        return isTrue;
    }
}